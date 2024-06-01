using FluentValidation;
using Microsoft.Extensions.Caching.Memory;
using SimpleOnlineStore.Api.DataAccess.IRepository;
using SimpleOnlineStore.Api.Domain.Dtos.Order.Requests;
using SimpleOnlineStore.Api.Domain.Dtos.Order.Responses;
using SimpleOnlineStore.Api.Domain.Dtos.Product.Responses;
using SimpleOnlineStore.Api.Domain.Dtos.User.Responses;
using SimpleOnlineStore.Api.Domain.Entities;
using SimpleOnlineStore.Api.Helper;
using SimpleOnlineStore.Api.Helper.ExceptionHandlers;
using SimpleOnlineStore.Api.IServices;

namespace SimpleOnlineStore.Api.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMemoryCache _memoryCache;
        private readonly IProductRepository _productRepository;
        private readonly IValidator<BuyProductRequestDto> _orderValidator;

        public OrderService(IOrderRepository orderRepository, IUserRepository userRepository, IMemoryCache memoryCache, IProductRepository productRepository, IValidator<BuyProductRequestDto> orderValidator)
        {
            _orderRepository = orderRepository;
            _userRepository = userRepository;
            _memoryCache = memoryCache;
            _productRepository = productRepository;
            _orderValidator = orderValidator;
        }
        public async Task<OrderResponseDto> CreateOrderAsync(BuyProductRequestDto buyProductRequestDto, CancellationToken cancellationToken)
        {
            await ValidateOrderAsync(buyProductRequestDto, cancellationToken);

            var user = await GetUserByIdAsync(buyProductRequestDto.UserId, cancellationToken);
            var product = await GetProductByIdAsync(buyProductRequestDto.ProductId, cancellationToken);

            InvalidateProductCache(buyProductRequestDto.ProductId);

            var createOrder = CreateOrder(user, product);

            var addOrder = await AddOrderAsync(createOrder, cancellationToken);

            return MapToOrderResponseDto(addOrder, product, user);
        }

        private async Task ValidateOrderAsync(BuyProductRequestDto buyProductRequestDto, CancellationToken cancellationToken)
        {
            var validationResult = await _orderValidator.ValidateAsync(buyProductRequestDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
        }

        private async Task<User> GetUserByIdAsync(int userId, CancellationToken cancellationToken)
        {
            return await _userRepository.GetByIdAsync(userId, cancellationToken)
                   ?? throw new KeyNotFoundException("User not found.");
        }

        private async Task<Product> GetProductByIdAsync(int productId, CancellationToken cancellationToken)
        {
            return await _productRepository.GetByIdAsync(productId, cancellationToken)
                   ?? throw new ProductNotFundException();
            //throw new KeyNotFoundException("Product not found.");
        }

        private void InvalidateProductCache(int productId)
        {
            _memoryCache.Remove($"Product-{productId}");
        }

        private Order CreateOrder(User user, Product product)
        {
            return new Order
            {
                Product = product,
                CreationDate = DateTime.Now,
                Buyer = user
            };
        }

        private async Task<int> AddOrderAsync(Order order, CancellationToken cancellationToken)
        {
            return await _orderRepository.AddAsync(order, cancellationToken);
        }

        private OrderResponseDto MapToOrderResponseDto(int orderId, Product product, User user)
        {
            return new OrderResponseDto
            {
                Id = orderId,
                Product = new GetProductResponseDto
                {
                    Discount = product.Discount,
                    PriceAfterDiscount = ProductHelper.CalculateProductDiscount(product.Price, product.Discount),
                    Id = product.Id,
                    Price = product.Price,
                    InventoryCount = product.InventoryCount,
                    Title = product.Title
                },
                CreationDate = DateTime.Now,
                Buyer = new UserResponseDto
                {
                    Name = user.Name,
                    Id = user.Id
                }
            };
        }
    }
}

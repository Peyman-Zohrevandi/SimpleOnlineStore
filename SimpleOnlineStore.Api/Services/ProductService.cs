using Microsoft.Extensions.Caching.Memory;
using FluentValidation;
using SimpleOnlineStore.Api.Domain.Dtos.Inventory.Requests;
using SimpleOnlineStore.Api.Domain.Dtos.Order.Requests;
using SimpleOnlineStore.Api.Domain.Dtos.Order.Responses;
using SimpleOnlineStore.Api.Domain.Dtos.Product.Requests;
using SimpleOnlineStore.Api.Domain.Dtos.Product.Responses;
using SimpleOnlineStore.Api.Helper;
using SimpleOnlineStore.Api.IServices;
using SimpleOnlineStore.Api.DataAccess.IRepository;
using SimpleOnlineStore.Api.Domain.Entities;
using SimpleOnlineStore.Api.Helper.ExceptionHandlers;

namespace SimpleOnlineStore.Api.Services
{
    public class ProductService : IProductService
    {
        private readonly IMemoryCache _cache;
        private readonly IValidator<AddProductRequestDto> _productValidator;
        private readonly IProductRepository _productRepository;
        private readonly IUserService _userService;
        private readonly IOrderService _orderService;

        public ProductService(IMemoryCache cache, IValidator<AddProductRequestDto> productValidator, IProductRepository productRepository, IUserService userService, IOrderService orderService)
        {
            _cache = cache;
            _productValidator = productValidator;
            _productRepository = productRepository;
            _userService = userService;
            _orderService = orderService;
        }

        public async Task<AddProductResponseDto> AddProductAsync(AddProductRequestDto addProductDto,CancellationToken cancellationToken)
        {
            await ValidateProductAsync(addProductDto, cancellationToken);
            await EnsureTitleIsUniqueAsync(addProductDto.Title, cancellationToken);

            var addProduct = await _productRepository.AddAsync(addProductDto, cancellationToken);

            return new AddProductResponseDto(addProduct.Id, addProduct.Title, addProduct.InventoryCount, addProduct.Price, addProduct.Discount);
        }
        
        public async Task<GetProductResponseDto> GetProductByIdAsync(GetProductByIdRequestDto getProductByIdDto, CancellationToken cancellationToken)
        {
            var cacheKey = $"Product-{getProductByIdDto.ProductId}";
            
            if (!_cache.TryGetValue(cacheKey, out GetProductResponseDto? productDto))
            {
               var product = await _productRepository.GetByIdAsync(getProductByIdDto.ProductId, cancellationToken);
                
                if (product == null)
                {
                    throw new ProductNotFundException();
                }
                
                productDto = MapToProductResponseDto(product);
            
                CacheProductResponse(cacheKey, productDto);
            }

            return productDto!;
        }
        
        public async Task<OrderResponseDto> BuyProductAsync(BuyProductRequestDto buyProductDto, CancellationToken cancellationToken)
        {
            await _userService.ValidateUserAsync(buyProductDto.UserId, cancellationToken);

            await ValidateProductAsync(buyProductDto.ProductId, cancellationToken);
            
            await ReduceInventoryAsync(buyProductDto.ProductId, cancellationToken);
            
            var order = await _orderService.CreateOrderAsync(buyProductDto, cancellationToken);
            
            return order;
        }

        public async Task ValidateProductAsync(int productId, CancellationToken cancellationToken)
        {
            var product =  await _productRepository.GetByIdAsync(productId, cancellationToken);
            if (product == null)
            {
                throw new ProductNotFundException();
            }

            if (product.InventoryCount <= 0)
            {
                throw new InvalidOperationException("Product is out of stock.");
            }
        }
        
        public async Task ReduceInventoryAsync(int productId, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(productId, cancellationToken);
            if (product == null)
            {
                throw new ProductNotFundException();
            }

            if (product.InventoryCount <= 0)
            {
                throw new InvalidOperationException("Product is out of stock.");
            }

            product.InventoryCount--;
            await _productRepository.UpdateAsync(product, cancellationToken);
        }

        public async Task<GetProductResponseDto> UpdateInventoryCountAsync(UpdateInventoryCountRequestDto updateInventoryCountDto, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(updateInventoryCountDto.ProductId, cancellationToken);
            
            if (product == null)
                throw new ProductNotFundException();

            _cache.Remove($"Product-{updateInventoryCountDto.ProductId}");

            product.InventoryCount += updateInventoryCountDto.InventoryCount;

            await _productRepository.UpdateAsync(product, cancellationToken);
            
            return new GetProductResponseDto
            {
                Discount = product.Discount,
                Id = product.Id,
                Price = product.Price,
                InventoryCount = product.InventoryCount,
                Title = product.Title,
                PriceAfterDiscount = ProductHelper.CalculateProductDiscount(product.Price,product.Discount)
            };

        }

        private GetProductResponseDto MapToProductResponseDto(Product product)
        {
            var priceAfterDiscount = ProductHelper.CalculateProductDiscount(product.Price, product.Discount);
            return new GetProductResponseDto
            {
                Discount = product.Discount,
                PriceAfterDiscount = priceAfterDiscount,
                Price = product.Price,
                Id = product.Id,
                InventoryCount = product.InventoryCount,
                Title = product.Title
            };
        }

        private void CacheProductResponse(string cacheKey, GetProductResponseDto productDto)
        {
            var cacheOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(5));

            _cache.Set(cacheKey, productDto, cacheOptions);
        }

        private async Task ValidateProductAsync(AddProductRequestDto addProductDto, CancellationToken cancellationToken)
        {
            var validationResult = await _productValidator.ValidateAsync(addProductDto, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
        }

        private async Task EnsureTitleIsUniqueAsync(string title, CancellationToken cancellationToken)
        {
            if (await _productRepository.IsTitleUniqueAsync(title, cancellationToken))
            {
                throw new ArgumentException("Product title must be unique.");
            }
        }
    }



}

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using SimpleOnlineStore.Api.DataAccess.Data;
using SimpleOnlineStore.Api.DataAccess.Repository;
using SimpleOnlineStore.Api.Domain.Dtos.Order.Requests;
using SimpleOnlineStore.Api.Domain.Dtos.Product.Requests;
using SimpleOnlineStore.Api.Domain.Entities;
using SimpleOnlineStore.Api.Services;
using SimpleOnlineStore.Api.Validators;
using Xunit;

namespace SimpleOnlineStore.Tests
{
    public class ProductServiceTests
    {
        private readonly StoreContext _context;
        private readonly ProductService _service;

        public ProductServiceTests()
        {
            var options = new DbContextOptionsBuilder<StoreContext>()
                .UseInMemoryDatabase(databaseName: "StoreTestDb")
                .Options;

            _context = new StoreContext(options);
            IMemoryCache cache = new MemoryCache(new MemoryCacheOptions());

            var productValidator = new ProductValidator();
            var userValidator = new UserValidator();
            var orderValidator = new OrderValidator();
            var productRepository = new ProductRepository(_context);
            var userService = new UserService(new UserRepository(_context), userValidator);
            var orderRepository = new OrderRepository(_context);
            var userRepository = new UserRepository(_context);
            var orderService = new OrderService(orderRepository, userRepository, cache, productRepository, orderValidator);

            _service = new ProductService(cache, productValidator, productRepository,
                userService,orderService);
        }

        [Fact]
        public async Task AddProductAsync_ShouldAddProduct_WhenProductIsValid()
        {
            
            // Arrange
            var cancellationToken = new CancellationToken();
            var addProductRequest = new AddProductRequestDto("Test Add Product", 10, 100, 10);

            // Act
            var result = await _service.AddProductAsync(addProductRequest, cancellationToken);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test Add Product", result.Title);
        }

        [Fact]
        public async Task BuyProductAsync_ShouldReduceInventoryCountAndCreateOrder_WhenProductIsInStock()
        {
            // Arrange
            var user = new User { Name = "Test User" };
            var product = new Product { Title = "Test Product", InventoryCount = 10, Price = 100, Discount = 10 };
            var cancellationToken = new CancellationToken();

            _context.Users.Add(user);
            _context.Products.Add(product);
            await _context.SaveChangesAsync(cancellationToken);

            var buyProductRequest = new BuyProductRequestDto(user.Id, product.Id);

            // Act
            var order = await _service.BuyProductAsync(buyProductRequest, cancellationToken);

            // Assert
            Assert.NotNull(order);
            Assert.Equal(9, product.InventoryCount);
            Assert.Single(user.Orders);
        }
    }
}

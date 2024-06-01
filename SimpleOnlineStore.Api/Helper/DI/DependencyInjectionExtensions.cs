using FluentValidation;
using SimpleOnlineStore.Api.DataAccess.IRepository;
using SimpleOnlineStore.Api.DataAccess.Repository;
using SimpleOnlineStore.Api.Domain.Dtos.Order.Requests;
using SimpleOnlineStore.Api.Domain.Dtos.Product.Requests;
using SimpleOnlineStore.Api.Domain.Entities;
using SimpleOnlineStore.Api.IServices;
using SimpleOnlineStore.Api.Services;
using SimpleOnlineStore.Api.Validators;

namespace SimpleOnlineStore.Api.Helper.DI
{

    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddSimpleStoreServices(this IServiceCollection services)
        {
            services.AddScoped<IProductService, ProductService>();
            services.AddMemoryCache();
            services.AddScoped<IValidator<User>, UserValidator>();
            services.AddScoped<IValidator<AddProductRequestDto>, ProductValidator>();
            services.AddScoped<IValidator<BuyProductRequestDto>, OrderValidator>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRepository, UserRepository>();
            return services;
        }
    }
}

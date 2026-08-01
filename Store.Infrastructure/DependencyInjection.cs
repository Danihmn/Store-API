using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Store.Domain.Repositories;
using Store.Domain.Secutiry;
using Store.Infrastructure.Data.StoreContext;
using Store.Infrastructure.Repositories;
using Store.Infrastructure.Security.Services;

namespace Store.Infrastructure;

public static class DependencyInjection
{
    public static WebApplicationBuilder AddInfrastructure(this WebApplicationBuilder builder)
    {
        var jwtKey = builder.Configuration["JwtSecretKey"]
                     ?? throw new InvalidOperationException("JwtSecretKey not found in configuration");

        builder.AddNpgsqlDbContext<StoreContext>("store");

        builder.Services.AddSingleton<ITokenService>(new TokenService(jwtKey));

        builder.Services.AddTransient<ICustomerRepository, CustomerRepository>();
        builder.Services.AddTransient<IProductRepository, ProductRepository>();
        builder.Services.AddTransient<IAddressRepository, AddressRepository>();
        builder.Services.AddTransient<IStoreRepository, StoreRepository>();
        builder.Services.AddTransient<IOrderRepository, OrderRepository>();
        builder.Services.AddTransient<IOrderProductRepository, OrderProductRepository>();
        builder.Services.AddTransient<IUserRepository, UserRepository>();

        return builder;
    }
}
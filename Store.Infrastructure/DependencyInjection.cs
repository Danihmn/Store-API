using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Store.Domain.Repositories;
using Store.Domain.Secutiry;
using Store.Infrastructure.Repositories;
using Store.Infrastructure.Security.Services;

namespace Store.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure (this IServiceCollection services, IConfiguration configuration)
    {
        var jwtKey = configuration["JwtSecretKey"]
            ?? throw new InvalidOperationException("JwtSecretKey not found in configuration");

        services.AddSingleton<ITokenService>(new TokenService(jwtKey));

        services.AddTransient<ICustomerRepository, CustomerRepository>();
        services.AddTransient<IProductRepository, ProductRepository>();
        services.AddTransient<IAddressRepository, AddressRepository>();
        services.AddTransient<IStoreRepository, StoreRepository>();
        services.AddTransient<IOrderRepository, OrderRepository>();
        services.AddTransient<IOrderProductRepository, OrderProductRepository>();

        return services;
    }
}

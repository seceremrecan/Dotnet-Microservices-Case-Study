using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Product.Application.Abstractions.Caching;
using Product.Application.Abstractions.Messaging;
using Product.Application.Abstractions.Persistence;
using Product.Infrastructure.Caching;
using Product.Infrastructure.Messaging;
using Product.Infrastructure.Persistence;
using Product.Infrastructure.Repositories;
using StackExchange.Redis;

namespace Product.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddProductInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ProductDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("ProductDb")));

        services.AddSingleton<IConnectionMultiplexer>(_ =>
            ConnectionMultiplexer.Connect(configuration.GetConnectionString("Redis")));

        services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((context, cfg) =>
            {
                var rabbitMqSection = configuration.GetSection("RabbitMq");

                cfg.Host(
                    rabbitMqSection["Host"]!,
                    rabbitMqSection["VirtualHost"]!,
                    h =>
                    {
                        h.Username(rabbitMqSection["Username"]!);
                        h.Password(rabbitMqSection["Password"]!);
                    });
            });
        });

        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IProductCacheService, RedisProductCacheService>();
        services.AddScoped<IProductEventPublisher, RabbitMqProductEventPublisher>();

        return services;
    }
}
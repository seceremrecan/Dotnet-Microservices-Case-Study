using Log.Application.Abstractions.Persistence;
using Log.Infrastructure.Consumers;
using Log.Infrastructure.Persistence;
using Log.Infrastructure.Repositories;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Log.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddLogInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<LogDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("LogDb")));

        services.AddScoped<ILogRepository, LogRepository>();

        services.AddMassTransit(x =>
        {
            x.AddConsumer<ProductCreatedEventConsumer>();
            x.AddConsumer<ProductUpdatedEventConsumer>();

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

                cfg.ReceiveEndpoint("product-created-log-queue", e =>
                {
                    e.ConfigureConsumer<ProductCreatedEventConsumer>(context);
                });

                cfg.ReceiveEndpoint("product-updated-log-queue", e =>
                {
                    e.ConfigureConsumer<ProductUpdatedEventConsumer>(context);
                });
            });
        });

        return services;
    }
}
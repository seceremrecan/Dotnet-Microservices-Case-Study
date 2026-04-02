using Log.Application.Abstractions.Persistence;
using Log.Infrastructure.Persistence;
using Log.Infrastructure.Repositories;
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

        return services;
    }
}

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Ordering.Data;
using Shared.Extensions;
using Shared.Data.Interceptors;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Ordering;

public static class OrderingModule
{
    public static IServiceCollection AddOrderingModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Data - Infrastructure Services

        var connectionString =
            configuration.GetConnectionString("Database");

        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

        services.AddDbContext<OrderingDbContext>((sp, options) =>
        {
            options.AddInterceptors(
                sp.GetServices<ISaveChangesInterceptor>());

            options.UseNpgsql(connectionString);
        });

        return services;
    }

    public static WebApplication UseOrderingModule(
        this WebApplication app)
    {
        // Apply database migrations
        app.UseMigrations<OrderingDbContext>();

        return app;
    }
}

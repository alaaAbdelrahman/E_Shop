using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Catalog.Data;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using Catalog.Data.Seed;
using Shared.Seed;
using Shared.Data.Interceptors;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Reflection;
using Shared.Extensions;
using Shared.Behaviors;
using FluentValidation;

namespace Catalog;


public static  class CatalogModule
{
    public static IServiceCollection AddCatalogModule(this IServiceCollection services, IConfiguration configuration)
    {
        // add services to the conatiner 

        // API Endpiont Services

        // Application Use Case services
        // 
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
            cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));

        });
        services .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        //Data - infrastructure services

        var connectionString = configuration.GetConnectionString("Database");
        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();
        services.AddDbContext<CatalogDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            options.UseNpgsql(connectionString);
        });
        services.AddScoped<IDataSeeder, CatalogDataSeeder>();

        return services;
    }

    public static WebApplication UseCatalogModule(this WebApplication app)
    {
        app.UseMigrations<CatalogDbContext>();
        
        
        return app;
    }

    
}

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;

namespace Catalog;


public static  class CatalogModule
{
    public static IServiceCollection AddCatalogModule(this IServiceCollection services, IConfiguration configuration)
    {
       // services.AddScoped<ICatalogService, CatalogService>();
        //services.AddScoped<ICatalogRepository, CatalogRepository>();

        return services;
    }

    public static WebApplication  UseCatalogModule(this WebApplication app)
    {
        return app;
    }
    
}

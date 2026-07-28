using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;


namespace Basket;

    public static class BasketModule
    {
        public static IServiceCollection AddBasketModule(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddScoped<IBasketService, BasketService>();
            //services.AddScoped<IBasketRepository, BasketRepository>();

            return services;
        }

        public static WebApplication   UseBasketModule(this WebApplication app)
        {
            return app;
        }
        
    }

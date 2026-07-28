using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;


namespace Ordering;


    public static class OrderingModule
    {
        public static IServiceCollection AddOrderingModule(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddScoped<IOrderingService, OrderingService>();
            //services.AddScoped<IOrderingRepository, OrderingRepository>();

            return services;
        }

        public static WebApplication  UseOrderingModule(this WebApplication app)
        {
            return app;
        }
    }

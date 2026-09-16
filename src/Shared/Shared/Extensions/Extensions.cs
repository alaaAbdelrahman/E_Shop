using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shared.Seed;

namespace Shared.Extensions;

public static  class Extensions
{
    public static WebApplication UseMigrations<TContext>(this WebApplication app) where TContext : DbContext
    {
      MigrateDatabaseAsync<TContext>(app.Services).GetAwaiter().GetResult() ;
       SeedDataAsync<TContext>(app.Services).GetAwaiter().GetResult();
        return app;
    }

    private static async Task SeedDataAsync<TContext>(IServiceProvider services) where TContext : DbContext
    {
        using var scope = services.CreateScope();
        var seeder = scope.ServiceProvider.GetService<IDataSeeder>();
        if (seeder != null)
        {
            await seeder.SeedAllAsync();
        }
    }

    private static async Task MigrateDatabaseAsync<TContext>(IServiceProvider services) where TContext : DbContext
    {
        using var scope = services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<TContext>();
        await dbContext.Database.MigrateAsync();
    }
}

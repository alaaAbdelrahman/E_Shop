using Catalog.Products.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Catalog.Data;

public class CatalogDbContext : DbContext
{
    public CatalogDbContext(DbContextOptions<CatalogDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // for data isolation between each module , we create a schema for each module
        // to define schema for each module, we can use the module name as the schema name
        modelBuilder.HasDefaultSchema("Catalog");
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}

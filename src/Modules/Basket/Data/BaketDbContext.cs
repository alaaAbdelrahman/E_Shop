using Basket.Basket.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Data;

public class BaketDbContext: DbContext
{
    public BaketDbContext(DbContextOptions<BaketDbContext> options)
                : base(options) { }
    
       public DbSet<ShoppingCart> ShoppingCarts => Set<ShoppingCart>();
     public DbSet<ShoppingCartItem> ShoppingCartItems => Set<ShoppingCartItem>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("basket");
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
        
    }
}



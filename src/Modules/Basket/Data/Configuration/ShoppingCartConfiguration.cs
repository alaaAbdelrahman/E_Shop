using Basket.Basket.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Data.Configuration;

public class ShoppingCartConfiguration : IEntityTypeConfiguration<ShoppingCart>
{
    public void Configure(EntityTypeBuilder<ShoppingCart> builder)
    {
        builder.HasKey(e => e.Id);
        builder.HasIndex(e => e.UserName).IsUnique();
        builder.Property(e => e.UserName).IsRequired();
        builder.HasMany(e => e.Items)
              .WithOne()
              .HasForeignKey(i => i.ShoppingCartId)
              .OnDelete(DeleteBehavior.Cascade);
    }
}

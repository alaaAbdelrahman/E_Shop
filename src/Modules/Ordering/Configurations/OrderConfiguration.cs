
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Models;

namespace Ordering.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.CustomerId).IsRequired();
        builder.HasIndex(e => e.OrderName).IsUnique();
        builder.OwnsOne(e => e.ShippingAddress);
        builder.OwnsOne(e => e.Payment);
        builder.HasMany(e => e.Items)
              .WithOne()
              .HasForeignKey("OrderId")
              .OnDelete(DeleteBehavior.Cascade);
    }
}

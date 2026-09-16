using Catalog.Products.Models;

namespace Catalog.Data;

public static class InitialData
{
    public static IEnumerable<Product> Products =>
        new List<Product>
        {
            Product.Create(
                Guid.NewGuid(),
                "iPhone 15 Pro",
                new List<string> { "Electronics", "Smartphones" },
                "Apple iPhone 15 Pro 256GB",
                999.99m,
                "https://example.com/images/iphone15pro.jpg"
            ),

            Product.Create(
                Guid.NewGuid(),
                "Samsung Galaxy S25",
                new List<string> { "Electronics", "Smartphones" },
                "Samsung Galaxy S25 Ultra",
                1099.99m,
                "https://example.com/images/galaxy-s25.jpg"
            ),

            Product.Create(
                Guid.NewGuid(),
                "MacBook Pro M4",
                new List<string> { "Electronics", "Laptops" },
                "Apple MacBook Pro 14-inch M4",
                2199.99m,
                "https://example.com/images/macbook.jpg"
            )
        };
}
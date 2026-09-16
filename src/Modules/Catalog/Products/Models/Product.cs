using Shared.DDD;
using Catalog.Products.Events;
namespace Catalog.Products.Models;

    public class Product:Aggregate<Guid>
    {
        public string Name { get; private set; } = default!;
        public List<string> Category{ get; private set; } = new List<string>();

        public string Description { get; private set; } = default!;

        public decimal Price { get; private  set; }

        public string ImageUrl { get; set; } = default!;


        public static Product Create(Guid id ,string name, List<string> category, string description, decimal price, string imageUrl)
        {
        
            ArgumentException.ThrowIfNullOrEmpty(name, nameof(name));
            ArgumentOutOfRangeException.ThrowIfNegative(price, nameof(price));
            var product = new Product
            {
                Id = id,
                Name = name,
                Category = category,
                Description = description,
                Price = price,
                ImageUrl = imageUrl,
                CreatedAt = DateTime.UtcNow
            };

            product.AddDomainEvent(new ProductCreatedEvent(product));
            return product;

        }

        public void Update(string name, List<string> category, string description, decimal price, string imageUrl)
        {
            ArgumentException.ThrowIfNullOrEmpty(name, nameof(name));
            ArgumentOutOfRangeException.ThrowIfNegative(price, nameof(price));

            Name = name;
            Category = category;
            Description = description;
            ImageUrl = imageUrl;
            LastModifiedAt = DateTime.UtcNow;
            if (Price != price)
            {
                            Price = price;

                AddDomainEvent(new ProductPriceChangedEvent(this));
            }
        }
        
    }
// add a Create method for initailizing the product with all required properties and validation.
// Make property setters private to ensure immutability after creation.
// add an update method to allow updating the product's properties with validation.
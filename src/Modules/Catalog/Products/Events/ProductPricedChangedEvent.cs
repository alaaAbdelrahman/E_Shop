using Catalog.Products.Models;
namespace Catalog.Products.Events;

    public record ProductPricedChangedEvent ( Product Product):IDomainEvent;
    

using Shared.DDD;
using  Catalog.Products.Models;
namespace Catalog.Products.Events;

    public class ProductCreatedEvent( Product Product):IDomainEvent;

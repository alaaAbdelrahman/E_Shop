using Catalog.Products.Events;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Messaging.Events;

namespace Catalog.Products.EventHandlers;

public class ProductPriceChangedEventHandler
    (IBus bus, ILogger<ProductPriceChangedEventHandler> logger) : INotificationHandler<ProductPriceChangedEvent>
{
    public async Task Handle(ProductPriceChangedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("ProductPriceChangedEventHandler: Product price changed for product {ProductId} to {NewPrice}", notification.Product.Id, notification.Product.Price);
        var integrationEVevent = new ProductPriceChangedIntegrationEvent
        {
            ProductId = notification.Product.Id,
            Name = notification.Product.Name,
            Category = notification.Product.Category,
            Description = notification.Product.Description,
            ImageUrl = notification.Product.ImageUrl,
            Price = notification.Product.Price
        };

        await bus.Publish(integrationEVevent, cancellationToken);
    }
}

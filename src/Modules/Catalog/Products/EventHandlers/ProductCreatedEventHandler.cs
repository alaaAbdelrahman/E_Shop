using Catalog.Products.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Products.EventHandlers;

public  class ProductCreatedEventHandler(ILogger<ProductCreatedEventHandler> logger)
    : INotificationHandler<ProductCreatedEvent>
{
    public Task Handle(ProductCreatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Product created event type: {EventType}", notification.GetType().Name);
        // Additional logic can be added here, such as sending notifications, updating other systems, etc.
        return Task.CompletedTask;
    }
}

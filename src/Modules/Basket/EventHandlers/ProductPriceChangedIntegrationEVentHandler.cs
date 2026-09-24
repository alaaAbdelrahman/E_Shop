

using Basket.Features.UpdateItemPriceInBasket;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Messaging.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Basket.EventHandlers;

public class ProductPriceChangedIntegrationEventHandler 
    (ISender sender,
    ILogger<ProductPriceChangedIntegrationEventHandler> logger)
    : IConsumer<ProductPriceChangedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<ProductPriceChangedIntegrationEvent> context)
    {
        // find basket items that contain the product and update their price
        logger.LogInformation("ProductPriceChangedIntegrationEventHandler: Product price changed for product {ProductId} to {Price}", context.Message.ProductId, context.Message.Price);

        var command = new UpdateItemPriceInBasketCommand(context.Message.ProductId, context.Message.Price);

        var result = await sender.Send(command);
        if(!result.IsSucces)
        {
            logger.LogError("ProductPriceChangedIntegrationEventHandler: Failed to update item price in basket for product {ProductId}", context.Message.ProductId);
        }

  logger.LogInformation("ProductPriceChangedIntegrationEventHandler: Successfully updated item price in basket for product {ProductId}", context.Message.ProductId);

    }
}

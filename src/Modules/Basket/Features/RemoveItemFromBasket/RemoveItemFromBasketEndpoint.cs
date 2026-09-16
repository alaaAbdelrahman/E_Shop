using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Features.RemoveItemFromBasket;

public class RemoveItemFromBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/basket/{userName}/items/{ProductId}", 
            async ([FromRoute]string userName,
                   [FromRoute]Guid ProductId, 
                   ISender sender) =>
        {
            var command = new RemoveItemFromBasketCommand(userName, ProductId);
            var result = await sender.Send(command);
            return  Results.Ok(result) ;
        }).Produces<RemoveItemFromBasketResponse>(StatusCodes.Status200OK)
          .ProducesProblem(StatusCodes.Status404NotFound)
          .WithSummary("Removes an item from a shopping basket")
          .WithDescription("This endpoint allows you to remove an item from a shopping basket.");
    }
}

public  record  RemoveItemFromBasketResponse(bool Success);
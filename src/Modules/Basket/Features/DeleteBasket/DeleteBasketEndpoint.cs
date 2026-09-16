using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Features.DeleteBasket;

public record DeleteBasketResponse(bool Success);
public class DeleteBasketEndpoint: ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/basket/{userName}", async (string userName, ISender sender) =>
        {
            var command = new DeleteBasketCommand(userName);
            var response = await sender.Send(command);
            return   Results.Ok(response);
        }).Produces<DeleteBasketResponse>(StatusCodes.Status200OK)
          .ProducesProblem(StatusCodes.Status404NotFound)
          .WithSummary("Deletes a shopping basket")
          .WithDescription("This endpoint allows you to delete a shopping basket.");
    }
}

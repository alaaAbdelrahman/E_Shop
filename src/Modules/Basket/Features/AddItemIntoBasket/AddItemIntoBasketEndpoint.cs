using Basket.Dtos;
using Carter;
using Mapster;
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

namespace Basket.Features.AddItemIntoBasket;

public record AddItemIntoBasketRequest( ShopingCartItemDto Item);

public record AddItemIntoBasketResponse(Guid id);

public class AddItemIntoBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/basket/{userName}/Items", async ([FromRoute]string userName,
            [FromBody]AddItemIntoBasketRequest request,
            ISender sender) =>
        {
            var command = new AddItemIntoBasketCommand(userName, request.Item);
            var result  = await sender.Send(command);
            var response = result.Adapt<AddItemIntoBasketResponse>();
            return Results.Created($"/basket/{response.id}", response);
        }).Produces<AddItemIntoBasketResponse>(StatusCodes.Status200OK)
          .WithSummary("Adds an item into a shopping basket")
          .WithDescription("This endpoint allows you to add an item into a shopping basket.");

    }
}

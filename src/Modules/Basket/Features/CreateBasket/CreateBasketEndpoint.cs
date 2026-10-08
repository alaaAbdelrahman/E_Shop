using Basket.Dtos;
using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Features.CreateBasket;


public record CreateBasketRequest(ShoppingCartDto ShoppingCart);

public record CreateBasketResponse(Guid Id);
public class CreateBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/basket", async (CreateBasketRequest request, ISender sender, ClaimsPrincipal user) =>
        {
            var userName = user.Identity!.Name;
            var updatedShoppingCart = request.ShoppingCart with {UserName = userName };
            var command = new CreateBasketCommand(updatedShoppingCart);
            var response = await sender.Send(command);
            var result = response.Adapt<CreateBasketResponse>();
            return Results.Created($"/basket/{result.Id}", result);
        }).Produces<CreateBasketResponse>(StatusCodes.Status201Created)
           .ProducesProblem(StatusCodes.Status400BadRequest)
           .WithSummary("Creates a new shopping basket")
           .WithDescription("This endpoint allows you to create a new shopping basket.")
           .RequireAuthorization();
    }
}

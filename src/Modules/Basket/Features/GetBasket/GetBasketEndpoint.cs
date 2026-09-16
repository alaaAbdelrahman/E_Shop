using Basket.Dtos;
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

namespace Basket.Features.GetBasket;

public class GetBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/basket/{userName}", async ([FromRoute]string userName, ISender sender) =>
        {
            var query = new GetBasketQuery(userName);
            var response = await sender.Send(query);
            return response is not null ? Results.Ok(response) : Results.NotFound();
        }).Produces<ShoppingCartDto>(StatusCodes.Status200OK)
          .ProducesProblem(StatusCodes.Status404NotFound)
          .WithSummary("Retrieves a shopping basket")
          .WithDescription("This endpoint allows you to retrieve a shopping basket.");
    }
}

using Microsoft.AspNetCore.Http;
using Ordering.Dtos;
using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Ordering.Features.GetOrderById;

public record GetOrderByIdResponse(OrderDto Order);
public class GetOrderByIDEndpiont : ICarterModule 
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/orders/{id:guid}", async (Guid id, ISender sender) =>
        {
            var query = new GetOrderByIdQuery(id);
            var result = await sender.Send(query);
            if (result == null)
            {
                return Results.NotFound();
            }
            var response = result.Adapt<GetOrderByIdResponse>();

            return Results.Ok(response);
        })
        .WithName("GetOrderById")
        .Produces<GetOrderByIdResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Retrieves an order by ID")
        .WithDescription("Retrieves an order with the provided ID and returns the order details.")
    }
}

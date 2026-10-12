using Microsoft.AspNetCore.Http;
using Ordering.Dtos;
using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Ordering.Features.DeleteOrder;


public record DeleteOrderResponse(Guid Id);

public class  DeleteOrderEndpoint:ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/orders/{id:guid}", async (Guid id, ISender sender) =>
        {
            var command = new DeleteOrderCommand(id);
            var result = await sender.Send(command);
            var response = new DeleteOrderResponse(result.Id);
            return Results.Ok(response);
        })
        .WithName("DeleteOrder")
        .Produces<DeleteOrderResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Deletes an existing order")
        .WithDescription("Deletes an existing order with the provided ID and returns the deleted order's ID.")
        .WithOpenApi();
    }
}
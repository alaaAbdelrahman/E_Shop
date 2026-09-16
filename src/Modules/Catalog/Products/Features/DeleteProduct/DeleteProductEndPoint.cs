using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Products.Features.DeleteProduct;

public record DeleteProductEndPointResponse(bool IsSuccess);
public  class DeleteProductEndPoint:ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/products/{id}", async (Guid id, ISender sender) =>
        {
            var command = new DeleteProductCommand(id);
            var result = await sender.Send(command);
            var response = result.Adapt< DeleteProductEndPointResponse>();
            return Results.Ok(response);
        }).WithName("DeleteProduct")
        .Produces<DeleteProductEndPointResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Deletes a product")
        .WithDescription("Deletes a product with the specified ID.");
    }

    
}

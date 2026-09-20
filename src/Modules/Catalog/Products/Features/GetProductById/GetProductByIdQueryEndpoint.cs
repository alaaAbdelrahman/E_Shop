using Carter;
using Catalog.Contracts.Products;
using Catalog.Contracts.Products.Features.GetProductById;
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

namespace Catalog.Products.Features.GetProductById;

public record GetProductByIdResponse(ProductDto Product);
public class GetProductByIdQueryEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/{productId:guid}", async (Guid productId, ISender sender) =>
         {
                    var result = await sender.Send(new GetProductByIdQuery(productId));
             var response = new GetProductByIdResponse(result.ProductDto);
             return Results.Ok(response);
         }).WithName("GetProductById")
           .Produces<GetProductByIdResponse>(StatusCodes.Status200OK)
           .Produces(StatusCodes.Status404NotFound)
           .WithTags("Products")
           .WithSummary("Get a product by its ID");
    }
}

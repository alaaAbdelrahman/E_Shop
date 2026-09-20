using Carter;
using Catalog.Contracts.Products;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Catalog.Products.Features.GetProductByCategory;

public record GetProductByCategoryResponse(IEnumerable<ProductDto> Products);
public class GetProductByCategoryEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/category/{category}", async (string category, ISender sender) =>
        {
            var result = await sender.Send(new GetProductByCategoryQuery(category));
            var response = result.Adapt<GetProductByCategoryResponse>();
            return Results.Ok(response);
        }).WithName("GetProductByCategory")
          .Produces<GetProductByCategoryResponse>(StatusCodes.Status200OK)
          .Produces(StatusCodes.Status404NotFound)
          .WithTags("Products")
          .WithSummary("Get products by category ");
    }
}

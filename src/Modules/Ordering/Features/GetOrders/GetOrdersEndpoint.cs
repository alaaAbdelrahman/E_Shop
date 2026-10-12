using Microsoft.AspNetCore.Http;
using Ordering.Dtos;
using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Shared.Pagination;
namespace Ordering.Features.GetOrders;

public record GetOrdersResponse(PaginatedResult<OrderDto> Orders);

public class GetOrdersEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/orders", async ([AsParameters]PaginationRequest request, ISender sender) =>
        {
            var result = await sender.Send(new GetOrdersQuery(request));

            var response = result.Adapt<GetOrdersResponse>();
            return Results.Ok(response);
        })
        .WithName("GetOrders")
        .Produces<GetOrdersResponse>(StatusCodes.Status200OK)
        .ProduceProblem(StatusCodes.Status400BadRequest)
        .withSummary("Retrieves a paginated list of orders")
        .withDescription("Retrieves a paginated list of orders based on the provided page number and page size.")
        .WithOpenApi();
    }
}

using System.Collections.Generic;

namespace Shared.Pagination
{
    public record PaginationResult<T>(IEnumerable<T> Items, int PageNumber, int PageSize, int TotalCount);
}

using Mapster;
using Microsoft.EntityFrameworkCore;
using Ordering.Data;
using Ordering.Dtos;
using Shared.Contracts.CQRS;

namespace Ordering.Features.GetOrderById;

public record GetOrderByIdQuery(Guid OrderId) : 
    IQuery<GetOrderByIdQueryResult>;

public record GetOrderByIdQueryResult(OrderDto Order);

internal class GetOrderByIdHandler(OrderingDbContext dbContext)
    : IQueryHandler<GetOrderByIdQuery, GetOrderByIdQueryResult>
{
    public async Task<GetOrderByIdQueryResult> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var order = await dbContext.Orders
            .AsNoTracking()
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == request.OrderId, cancellationToken);

        if (order == null)
        {
            throw new NotFoundException($"Order with ID {request.OrderId} not found.");
        }

        var orderDto = order.Adapt<OrderDto>();
        return new GetOrderByIdQueryResult(orderDto);
    }
}

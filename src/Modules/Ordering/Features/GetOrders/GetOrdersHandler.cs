using Mapster;
using Microsoft.EntityFrameworkCore;
using Ordering.Data;
using Ordering.Dtos;
using Shared.Contracts.CQRS;
using Shared.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Features.GetOrders;

public  record GetOrdersQuery(PaginationRequest PaginationRequest) 
            : IQuery<GetOrdersResult>;

public record GetOrdersResult
(PaginatedResult<OrderDto> Orders);

internal class GetOrdersHandler(OrderingDbContext dbContext)
    : IQueryHandler<GetOrdersQuery, GetOrdersResult>
{
    public async Task<GetOrdersResult> Handle(GetOrdersQuery command, CancellationToken cancellationToken)
    {
        var pageIndex = command.PaginationRequest.PageIndex;
        var pageSize = command.PaginationRequest.PageSize;

        var totalCount = await dbContext.Orders.CountAsync(cancellationToken);

        var orders = await dbContext.Orders
            .AsNoTracking()
            .Include(o => o.Items)
            .OrderBy(p=>p.OrderName)
            .Skip(pageIndex * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
        var orderDtos = orders.Adapt<List<OrderDto>>();

        return new GetOrdersResult(
            new PaginatedResult<OrderDto>
        (
            pageIndex,
            pageSize,
            totalCount,
            orderDtos
        ));
    }
}

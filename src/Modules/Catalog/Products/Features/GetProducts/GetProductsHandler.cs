using Catalog.Data;
using Catalog.Products.Dtos;
using Catalog.Products.Models;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Shared.CQRS;
using Shared.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Products.Features.GetProducts;

public record GetProductsQuery(PaginationRequest request): IQuery<GetProductsResult>;



public record GetProductsResult(PaginatedResult<ProductDto> Products);

internal class GetProductsHandler(CatalogDbContext dbContext) : IQueryHandler<GetProductsQuery, GetProductsResult>
{
    public async Task<GetProductsResult> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var pageIndex = request.request.PageIndex;
        var pageSize = request.request.PageSize;

        var totalCount = await dbContext.Products.LongCountAsync(cancellationToken);


        var products = await dbContext.Products
            .AsNoTracking()
            .OrderBy(p => p.Name)
            .Skip((pageIndex ) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var ProductsDtos = products.Adapt<List<ProductDto>>();

        return new GetProductsResult(
            new PaginatedResult<ProductDto>(
                pageIndex, pageSize, (int)totalCount, ProductsDtos
                ));
    }

   
}

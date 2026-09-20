using Catalog.Contracts.Products;
using Catalog.Contracts.Products.Features.GetProductById;
using Catalog.Data;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Products.Features.GetProductById;

internal class GetProductByIdQueryHandler(CatalogDbContext dbContext) : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
{
    public async Task<GetProductByIdResult> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await dbContext.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == request.ProductId, cancellationToken);
        if (product == null) throw new Exception($"Product not found: {request.ProductId}");

        var productDto = product.Adapt<ProductDto>();
        return new GetProductByIdResult(productDto);
    }
}

using Catalog.Data;
using Catalog.Products.Dtos;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Shared.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Products.Features.GetProductById;

public record GetProductById(Guid ProductId) : IQuery<GetProductByIdResult>;
public record GetProductByIdResult(ProductDto ProductDto);
internal class GetProductByIdHandler(CatalogDbContext dbContext) : IQueryHandler<GetProductById, GetProductByIdResult>
{
    public async Task<GetProductByIdResult> Handle(GetProductById request, CancellationToken cancellationToken)
    {
        var product = await dbContext.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == request.ProductId, cancellationToken);
        if (product == null) throw new Exception($"Product not found: {request.ProductId}");

        var productDto = product.Adapt<ProductDto>();
        return new GetProductByIdResult(productDto);
    }
}

using Catalog.Data;
using Catalog.Products.Dtos;
using Catalog.Products.Models;
using Shared.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Products.Features.UpdateProduct;

public record UpdateProductCommand(ProductDto Product) :
    ICommand<UpdateProductResult>;

public record UpdateProductResult(bool IsSuccess);

internal class UpdateProductHandler (CatalogDbContext  dbContext):
    ICommandHandler<UpdateProductCommand, UpdateProductResult>
{
    public async Task<UpdateProductResult> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await dbContext.Products.FindAsync(request.Product.Id);
        if (product == null) return new UpdateProductResult(false);

        UpdateProduct(request.Product, product);
        dbContext.Products.Update(product);
        await dbContext.SaveChangesAsync(cancellationToken);
        return new UpdateProductResult(true);
    }

    private void UpdateProduct(ProductDto product1, Product product2)
    {
        product2.Update(product1.Name,
            product1.Category,
            product1.Description,
            product1.Price,
            product1.ImageFile);
    }
}
    
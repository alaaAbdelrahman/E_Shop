using Catalog.Data;
using Catalog.Products.Dtos;
using Catalog.Products.Models;
using FluentValidation;
using MediatR;
using Shared.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Products.Features.CreateProduct;


public record CreateProductCommand
    (ProductDto product )
    : ICommand<CreateProductResult>;
public record CreateProductResult(Guid Id);

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.product.Name).NotEmpty().WithMessage("Product name is required.");
        RuleFor(x => x.product.Category).NotEmpty().WithMessage("Product category is required.");
        RuleFor(x => x.product.Description).NotEmpty().WithMessage("Product description is required.");
        RuleFor(x => x.product.Price).GreaterThan(0).WithMessage("Product price must be greater than zero.");
        RuleFor(x => x.product.ImageFile).NotEmpty().WithMessage("Product image file is required.");
    }
}
public class CreateProductCommandHandler(CatalogDbContext dbContext)
    : IRequestHandler<CreateProductCommand, CreateProductResult>
{
    public async  Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        var product = CreateNewProduct(command.product);

        // save to database
        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync(cancellationToken);

        // return result
        return new CreateProductResult(product.Id); 
    }

    private Product CreateNewProduct(ProductDto product)
    {
        var newProduct = Product.Create(Guid.NewGuid(), product.Name, product.Category, product.Description, product.Price, product.ImageFile);
        return newProduct;
    }
}   

  
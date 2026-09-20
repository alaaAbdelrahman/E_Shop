using Basket.Data;
using Basket.Data.Repository;
using Basket.Dtos;
using Basket.Exceptions;
using Catalog.Contracts.Products.Features.GetProductById;
using FluentValidation;
using FluentValidation.Validators;
using MediatR;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Basket.Features.AddItemIntoBasket;

public record AddItemIntoBasketCommand(string UserName,ShopingCartItemDto shoppingCartItem)
    :ICommand<AddItemIntoBasketResult>;

public record AddItemIntoBasketResult(Guid Id);

public class AddItemIntoBasketCommandValidator : 
    AbstractValidator<AddItemIntoBasketCommand>
{
    public AddItemIntoBasketCommandValidator()
    { 
        RuleFor(x=>x.UserName).NotEmpty().WithMessage("UserName Is Required ");
        RuleFor(x => x.shoppingCartItem.ProductId).NotEmpty().WithMessage("ProductId is required");
        RuleFor(x => x.shoppingCartItem.Quantity).GreaterThan(0).WithMessage("Quantity is wrong");
    }

}


internal class AddItemIntoBasketHandler(IBasketRepository basketRepository, ISender sender)
    : ICommandHandler<AddItemIntoBasketCommand, AddItemIntoBasketResult>
{
    public async Task<AddItemIntoBasketResult> Handle(AddItemIntoBasketCommand command, CancellationToken cancellationToken)
    {
       
         var shoppingCart = await basketRepository.GetBasket(command.UserName,true, cancellationToken);
        if (shoppingCart is null)
        {
            throw new BasketNotFoundException(command.UserName);
        }
        var result = await sender.Send(new GetProductByIdQuery(command.shoppingCartItem.ProductId));
        if(result.ProductDto is null)
        {
            throw new Exception("Product not found");
        }
        shoppingCart.AddItem(
            command.shoppingCartItem.ProductId,
            command.shoppingCartItem.Quantity,
            command.shoppingCartItem.Color,
             result.ProductDto.Name,
            result.ProductDto.Price
             );

        await basketRepository.SaveChangesAsync(command.UserName,cancellationToken);
        return new  AddItemIntoBasketResult(shoppingCart.Id);


    }

}

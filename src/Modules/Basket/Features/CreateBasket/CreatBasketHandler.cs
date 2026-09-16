using Basket.Basket.Models;
using Basket.Data;
using Basket.Dtos;
using FluentValidation;
using Shared.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Features.CreateBasket;

public record CreateBasketCommand(ShoppingCartDto ShoppingCart):
    ICommand<CreateBasketResult>;

public record  CreateBasketResult(Guid Id);

public class CreateBasketCommandValidator : AbstractValidator<CreateBasketCommand>
{
    public CreateBasketCommandValidator()
    {
        RuleFor(x=>x.ShoppingCart.UserName).NotEmpty().WithMessage("UserName is Empty");
    }
}
internal class CreatBasketHandler (BaketDbContext dbContext): ICommandHandler<CreateBasketCommand, CreateBasketResult>
{
    public async Task<CreateBasketResult> Handle(CreateBasketCommand command, CancellationToken cancellationToken)
    {
        // create Basket entity from command object 
        // save to database 
        // return result

        var shoppingCart= CreateNewBasket(command.ShoppingCart);
        dbContext.ShoppingCarts.Add(shoppingCart);
        await dbContext.SaveChangesAsync(cancellationToken);
        return new CreateBasketResult(shoppingCart.Id);
    }

    private ShoppingCart CreateNewBasket(ShoppingCartDto shoppingCart)
    {
        var newBasket = ShoppingCart.Create(
            Guid.NewGuid(),
            shoppingCart.UserName
            );
        shoppingCart.Items.ForEach(
            item =>
            {
                newBasket.AddItem(
                    item.ProductId,
                    item.Quantity,
                    item.Color,
                    item.ProductName,
                    item.Price
                    );

            }
            );
        return newBasket;
    }
}

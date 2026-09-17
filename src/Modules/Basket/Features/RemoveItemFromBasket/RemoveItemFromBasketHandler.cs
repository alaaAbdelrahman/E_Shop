using Basket.Data;
using Basket.Data.Repository;
using Basket.Exceptions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Shared.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Basket.Features.RemoveItemFromBasket;

public record RemoveItemFromBasketCommand(string UserName,Guid ProductId)
    :ICommand<RemoveItemFromBasketResult>;

public record RemoveItemFromBasketResult(Guid Id);

public class RemoveItemFromBasketCommandValidator : AbstractValidator<RemoveItemFromBasketCommand>
{
    public RemoveItemFromBasketCommandValidator()
    {
        RuleFor(x => x.UserName).NotEmpty().WithMessage("UserName Is Required");
        RuleFor(x => x.ProductId).NotEmpty().WithMessage("ProductId is Required");
    }
}
internal class RemoveItemFromBasketHandler(IBasketRepository basket)
    : ICommandHandler<RemoveItemFromBasketCommand, RemoveItemFromBasketResult>
{
    public async  Task<RemoveItemFromBasketResult> Handle(RemoveItemFromBasketCommand Command, CancellationToken cancellationToken)
    {
        var shoppingCart = await basket.GetBasket(Command.UserName,false,cancellationToken);
        if (shoppingCart is null) {
            throw new BasketNotFoundException(Command.UserName);
        }
        shoppingCart.RemoveItem(Command.ProductId);

        await basket.SaveChangesAsync();
        return new RemoveItemFromBasketResult(shoppingCart.Id);

    }
}

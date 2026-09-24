using Basket.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Features.UpdateItemPriceInBasket;
public record UpdateItemPriceInBasketCommand(Guid ProductId, decimal NewPrice) 
    :ICommand<UpdateItemPriceInBasketResult>;

public record UpdateItemPriceInBasketResult(bool IsSucces);

public class UpdateItemPriceInBasketCommandValidator : AbstractValidator<UpdateItemPriceInBasketCommand>
{
    public UpdateItemPriceInBasketCommandValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.NewPrice).GreaterThan(0);
    }
}

internal class UpdateItemPriceInBasketHandler(BaketDbContext dbcontext )
    : ICommandHandler<UpdateItemPriceInBasketCommand, UpdateItemPriceInBasketResult>
{
    public async Task<UpdateItemPriceInBasketResult> Handle(UpdateItemPriceInBasketCommand command, CancellationToken cancellationToken)
    {
        // find Shopping cart Items with a given Product Id
        // iterate through the items and update the price
        // save the changes to the database
        // return a result indicating success or failure

        var itemsUpdate = await dbcontext.ShoppingCartItems
            .Where(x => x.ProductId == command.ProductId)
            .ToListAsync(cancellationToken);
        if(!itemsUpdate.Any())
        {
            return new UpdateItemPriceInBasketResult(false);
        }   
        foreach (var item in itemsUpdate)
        {
            item.updatePrice(command.NewPrice);
        }

        await dbcontext.SaveChangesAsync(cancellationToken);

        return new UpdateItemPriceInBasketResult(true);
    }
}

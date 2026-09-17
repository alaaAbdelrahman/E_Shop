using Basket.Data;
using Basket.Data.Repository;
using Basket.Exceptions;
using Microsoft.EntityFrameworkCore;
using Shared.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Basket.Features.DeleteBasket;

public record DeleteBasketCommand(string UserName)
    :ICommand<DeleteBasketResult>;

public record DeleteBasketResult(bool IsSuccess);

internal class DeleteBasketHandler(IBasketRepository basketRepository)
    : ICommandHandler<DeleteBasketCommand, DeleteBasketResult>
{
    public async Task<DeleteBasketResult> Handle(DeleteBasketCommand command, CancellationToken cancellationToken)
    {
        var basket = await basketRepository.GetBasket(command.UserName,true, cancellationToken);
        if (basket is null) { 
            throw new BasketNotFoundException(command.UserName);
        
        }
        basketRepository.DeleteBasket(basket.UserName, cancellationToken);
        await basketRepository.SaveChangesAsync(cancellationToken);
        return new DeleteBasketResult(true);
    }
}

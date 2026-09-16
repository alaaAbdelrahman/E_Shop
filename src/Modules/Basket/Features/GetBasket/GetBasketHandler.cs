using Basket.Data;
using Basket.Dtos;
using Basket.Exceptions;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Shared.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Features.GetBasket;
public record GetBasketQuery(string UserName)
        :IQuery<GetBasketResult>;

public record GetBasketResult(ShoppingCartDto ShoppingCart);


internal class GetBasketHandler(BaketDbContext dbContext)
    : IQueryHandler<GetBasketQuery, GetBasketResult>
{
    public async Task<GetBasketResult> Handle(GetBasketQuery query, CancellationToken cancellationToken)
    {
        var basket = await dbContext.ShoppingCarts
                    .AsNoTracking()
                    .Include(x => x.Items)
                    .SingleOrDefaultAsync(x => x.UserName == query.UserName, cancellationToken);
        if (basket is null)
        {
            throw new BasketNotFoundException(query.UserName);
        }

        var basketDto = basket.Adapt<ShoppingCartDto>();

        return new GetBasketResult(basketDto);
    }
}

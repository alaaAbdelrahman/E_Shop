using Basket.Basket.Models;
using Basket.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Basket.Data.Repository;

public class BasketRepository(BaketDbContext context)
    : IBasketRepository
{
    public async Task<ShoppingCart> CreatBasket(ShoppingCart  basket,bool asNoTracking = true , CancellationToken cancellationToken)
    {
        context.ShoppingCarts.Add(basket);
        await context.SaveChangesAsync(cancellationToken);
        return basket; 
    }

    public async Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken)
    {
        var basket = await GetBasket(userName, false, cancellationToken);
        if (basket != null)
        {
            context.ShoppingCarts.Remove(basket);
            await context.SaveChangesAsync(cancellationToken);
            return true;
        }
        return false;
    }

    public async Task<ShoppingCart> GetBasket(string userName, bool AsNoTracking = true, CancellationToken cancellationToken = default)
    {
        var query = context.ShoppingCarts
                    .Include(x => x.Items)
                    .Where(x => x.UserName == userName);

        if (AsNoTracking)
        {
            query = query.AsNoTracking();
        }

        var basket = await query.SingleOrDefaultAsync(cancellationToken);
        return basket ;
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellation = default)
    {
        return context.SaveChangesAsync(cancellation);
    }

    

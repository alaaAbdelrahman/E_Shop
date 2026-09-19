using Basket.Basket.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Data.Repository;

public  interface IBasketRepository
{
    Task<ShoppingCart> GetBasket(string userName, bool AsNoTracking , CancellationToken cancellationToken);
    Task<ShoppingCart> CreatBasket(ShoppingCart shoppingCart, CancellationToken cancellationToken);
    Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken);

    Task<int> SaveChangesAsync(string ? userName = null , CancellationToken cancellation = default);
}

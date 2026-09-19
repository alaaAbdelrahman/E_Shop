using Shared.DDD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Basket.Models;

public class ShoppingCart:Aggregate<Guid>
{
    public string UserName { get; private set; } = default;
    private readonly List<ShoppingCartItem> _items = new List<ShoppingCartItem>();

    public IReadOnlyCollection<ShoppingCartItem> Items => _items.AsReadOnly();
    public decimal TotalPrice => _items.Sum(x => x.Price * x.Quantity);

    public static ShoppingCart Create(Guid id, string userName)
    { 
        ArgumentException.ThrowIfNullOrEmpty(userName);

        var shoppingCart = new ShoppingCart
        {
            Id = id,
            UserName = userName
        };

        return shoppingCart;

    }

    public void AddItem(
        Guid productId,
        int quantity,
        string color,
        string productName,
        decimal price)
    {
        ArgumentException.ThrowIfNullOrEmpty(productName);
        ArgumentException.ThrowIfNullOrEmpty(color);
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(quantity, 0);
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(price, 0);

        var existingItem = _items.FirstOrDefault(
            x => x.ProductId == productId && x.Color == color);

        if (existingItem != null)
        {
            existingItem.UpdateQuantity(quantity);
        }
        else
        {
            var newItem = new ShoppingCartItem(
                Id,
                productId,
                quantity,
                color,
                productName,
                price);

            _items.Add(newItem);
        }
    }
    public void RemoveItem(Guid productId)
    {
        var existingItem = _items.FirstOrDefault(x => x.ProductId == productId );
        if (existingItem != null)
        {
            _items.Remove(existingItem);
        }
    }
}



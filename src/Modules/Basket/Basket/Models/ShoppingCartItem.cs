using Shared.DDD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Basket.Models;

public  class ShoppingCartItem:Entity<Guid>
{
    public Guid ShoppingCartId { get; private set; } = default!;
    public Guid ProductId { get;  set; } = default!;
    public int Quantity { get; private set; } = default!;
    public string Color { get; private set; } = default!;

    // will comes from catlog module 
    public string ProductName { get; private set; } = default!;
    public decimal Price { get; private set; } = default!;


    internal ShoppingCartItem(Guid shoppingCartId, Guid productId, int quantity, string color, string productName, decimal price)
    {
        ShoppingCartId = shoppingCartId;
        ProductId = productId;
        Quantity = quantity;
        Color = color;
        ProductName = productName;
        Price = price;
    }
}

using Intuit.Ipp.WebhooksService;
using Shared.DDD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Models;

public  class OrderItem:Entity<Guid>
{
    public Guid OrderId { get; private set; }=default!;
    public Guid ProductId { get; private set; }=default!;
    public int Quantity { get; private set; }=default!;
    public decimal Price { get; private set; }=default!;

    internal OrderItem(Guid orderId ,Guid productId, int quantity, decimal price)
    {
        OrderId = orderId;
        ProductId = productId;
        Quantity = quantity;
        Price = price;
    }

    internal void IncreaseQuantity(int quantity)
    {
        Quantity += quantity;
    }
}

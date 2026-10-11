
using Ordering.Events;
using Ordering.ValueObjects;
using Shared.DDD;

namespace Ordering.Models;

public class Order:Aggregate<Guid>
{
    private readonly List<OrderItem> _orderItems = new List<OrderItem>();
    public IReadOnlyList<OrderItem> Items => _orderItems.AsReadOnly();
    public Guid Id { get; private set; }=default!;
    public Guid CustomerId { get; private set; }=default!;
    public string OrderName { get; private set; }=string.Empty;

    public Address ShippingAddress { get; private set; }=default!;

    public Payment Payment { get; private set; }=default!;

    public decimal TotalPrice => Items.Sum(item => item.Price * item.Quantity);

    public static Order Create(Guid orderId,Guid customerId, string orderName, Address shippingAddress, Payment payment)
    {
        var order = new Order
        {
            Id = orderId,
            CustomerId = customerId,
            OrderName = orderName,
            ShippingAddress = shippingAddress,
            Payment = payment
        };

        order.AddDomainEvent(new OrderCreatedEvent(order));
        return order;
    }    
    public void Add(Guid productId, int quantity, decimal price)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity, nameof(quantity));
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price, nameof(price));

        var existingItem = _orderItems.FirstOrDefault(item => item.ProductId == productId);
        if (existingItem != null)
        {
            existingItem.IncreaseQuantity(quantity);
        }
        else
        {
            var orderItem = new OrderItem(Guid.NewGuid(), productId, quantity, price);
            _orderItems.Add(orderItem);
        }
    }
    void Remove(Guid productId)
    {
        var existingItem = _orderItems.FirstOrDefault(item => item.ProductId == productId);
        if (existingItem != null)
        {
            _orderItems.Remove(existingItem);
        }
    }

}

using FluentValidation;
using Ordering.Data;
using Ordering.Dtos;
using Ordering.Models;
using Ordering.ValueObjects;
using Shared.Contracts.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Features.CreateOrder;

public record CreateOrderCommand
(OrderDto Order) : ICommand<CreateOrderResult>;

public record CreateOrderResult(Guid Id);

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.Order).NotNull();
        RuleFor(x => x.Order.CustomerId).NotEmpty();
        RuleFor(x => x.Order.OrderName).NotEmpty();
        RuleFor(x => x.Order.ShippingAddress).NotNull();
        RuleFor(x => x.Order.BillingAddress).NotNull();
        RuleFor(x => x.Order.Payment).NotNull();
        RuleFor(x => x.Order.Items).NotEmpty();
    }

    
}

internal class CreateOrderHandler(OrderingDbContext dbContext)
    : ICommandHandler<CreateOrderCommand, CreateOrderResult>
{
    public async Task<CreateOrderResult> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        var order = CreateNewOrder(command.Order);
        dbContext.Orders.Add(order);
        await dbContext.SaveChangesAsync(cancellationToken);
        return new CreateOrderResult(order.Id);
    }

    private Order CreateNewOrder(OrderDto order)
    {
       var shippingAddress = Address.Of(order.ShippingAddress.FirstName, order.ShippingAddress.LastName, order.ShippingAddress.EmailAddress, order.ShippingAddress.AddressLine, order.ShippingAddress.Country, order.ShippingAddress.State, order.ShippingAddress.ZipCode);
        var newOrder = Order.Create(
            Guid.NewGuid(),
            order.CustomerId,
            $"{order.OrderName}_{new Random().Next()}",
            shippingAddress, 
            Payment.Of(order.Payment.CardName,order.Payment.CardNumber,order.Payment.Expiration,order.Payment.CVV,order.Payment.PaymentMethod));
        order.Items.ForEach(item => 
        newOrder.Add(item.ProductId, item.Quantity, item.Price));
        return newOrder;    
    }

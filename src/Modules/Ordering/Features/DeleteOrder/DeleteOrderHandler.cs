using FluentValidation;
using Ordering.Data;
using Shared.Contracts.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Ordering.Features.DeleteOrder;

public  record DeleteOrderCommand(Guid OrderId) 
    : ICommand<DeleteOrderResult>;

public record DeleteOrderResult(Guid OrderId);

public class DeleteOrderCommandValidator : AbstractValidator<DeleteOrderCommand>
{
    public DeleteOrderCommandValidator()
    {
        RuleFor(x => x.OrderId).NotEmpty();
    }
}
internal class DeleteOrderHandler(OrderingDbContext dbContext)
    : ICommandHandler<DeleteOrderCommand, DeleteOrderResult>
{
    public async Task<DeleteOrderResult> Handle(DeleteOrderCommand command, CancellationToken cancellationToken)
    {
        var order = await dbContext.Orders.
            FindAsync( [command.OrderId],cancellationToken);
        if (order is null)
        {
            throw new Exception($"Order with Id {command.OrderId} not found.");
        }
        dbContext.Orders.Remove(order);
        await dbContext.SaveChangesAsync(cancellationToken);
        return new DeleteOrderResult(command.OrderId);
    }
}

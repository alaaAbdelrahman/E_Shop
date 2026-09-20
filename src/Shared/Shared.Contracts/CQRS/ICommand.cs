using MediatR;


namespace Shared.Contracts.CQRS;

public  interface ICommand<out TResponse> : IRequest<TResponse> where TResponse : notnull
{
}

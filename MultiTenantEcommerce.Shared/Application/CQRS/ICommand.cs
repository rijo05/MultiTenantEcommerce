using MediatR;

namespace MultiTenantEcommerce.Shared.Application.CQRS;

public interface ICommand<TResponse> : IRequest<TResponse>
{
}

public interface ICommandHandler<TCommand, TResponse>
    : IRequestHandler<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{
}
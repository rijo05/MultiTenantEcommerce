using MediatR;

namespace MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
public interface ICommand<TResponse> : IRequest<TResponse> { }

public interface ICommandHandler<TCommand, TResponse>
    : IRequestHandler<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{ }
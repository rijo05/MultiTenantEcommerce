using MediatR;

namespace MultiTenantEcommerce.Shared.Application.CQRS;

public interface IQuery<TResponse> : IRequest<TResponse>
{
}

public interface IQueryHandler<TQuery, TResponse>
    : IRequestHandler<TQuery, TResponse>
    where TQuery : IQuery<TResponse>
{
}
using MultiTenantEcommerce.Shared.Domain.Events;
using MultiTenantEcommerce.Shared.Integration.Events;

namespace MultiTenantEcommerce.Shared.Infrastructure.Messaging;

public interface ISyncHandler<in TEvent> where TEvent : IDomainEvent
{
    Task HandleAsync(TEvent evt);
}

public interface IAsyncHandler<in TEvent> where TEvent : IIntegrationEvent
{
    Task HandleAsync(TEvent evt);
}
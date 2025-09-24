using MultiTenantEcommerce.Domain.Common.Events;

namespace MultiTenantEcommerce.Application.Common.Interfaces.Persistence;
public interface IEventHandler<TDomainEvent> where TDomainEvent : IDomainEvent
{
    public Task HandleAsync(TDomainEvent domainEvent);
}

using MultiTenantEcommerce.Shared.Domain.Events;
using MultiTenantEcommerce.Shared.Integration.Events;

namespace MultiTenantEcommerce.Shared.Application.Interfaces;
public interface IEventDispatcher
{
    Task DispatchAsync(IIntegrationEvent integrationEvent, CancellationToken ct = default);
    Task DispatchSync(IDomainEvent domainEvent, CancellationToken ct = default);
}
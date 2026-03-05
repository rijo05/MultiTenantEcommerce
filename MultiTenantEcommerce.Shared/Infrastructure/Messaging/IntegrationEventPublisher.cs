using MultiTenantEcommerce.Shared.Application.Interfaces;
using MultiTenantEcommerce.Shared.Integration.Events;

namespace MultiTenantEcommerce.Shared.Infrastructure.Messaging;

public class IntegrationEventPublisher : IIntegrationEventPublisher
{
    private readonly List<IIntegrationEvent> _events = new();

    public void AddEvent(IIntegrationEvent integrationEvent) => _events.Add(integrationEvent);
    public IReadOnlyCollection<IIntegrationEvent> GetAllEvents() => _events.AsReadOnly();
    public void ClearEvents() => _events.Clear();
}

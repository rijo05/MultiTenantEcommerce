using MultiTenantEcommerce.Shared.Integration.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Shared.Application.Events;
public class IntegrationEventPublisher : IIntegrationEventPublisher
{
    private readonly List<IIntegrationEvent> _events = new();

    public void AddEvent(IIntegrationEvent integrationEvent) => _events.Add(integrationEvent);
    public IReadOnlyCollection<IIntegrationEvent> GetAllEvents() => _events.AsReadOnly();
    public void ClearEvents() => _events.Clear();
}

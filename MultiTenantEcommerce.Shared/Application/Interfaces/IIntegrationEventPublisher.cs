using MultiTenantEcommerce.Shared.Integration.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Shared.Application.Interfaces;
public interface IIntegrationEventPublisher
{
    void AddEvent(IIntegrationEvent integrationEvent);
    IReadOnlyCollection<IIntegrationEvent> GetAllEvents();
    void ClearEvents();
}

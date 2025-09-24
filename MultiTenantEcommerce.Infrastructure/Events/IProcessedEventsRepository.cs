using MultiTenantEcommerce.Domain.Common.Interfaces;

namespace MultiTenantEcommerce.Infrastructure.Events;
public interface IProcessedEventsRepository : IRepository<ProcessedEvent>
{
    public Task<bool> WasThisEventProcessedAlready(Guid eventId, string handlerName);
}

using MultiTenantEcommerce.Domain.Common.Interfaces;
using MultiTenantEcommerce.Domain.Enums;

namespace MultiTenantEcommerce.Infrastructure.Outbox;
public interface IOutboxRepository : IRepository<OutboxEvent>
{
    public Task<List<OutboxEvent>> GetUnprocessedEvents(EventPriority priority, int batchSize);
}

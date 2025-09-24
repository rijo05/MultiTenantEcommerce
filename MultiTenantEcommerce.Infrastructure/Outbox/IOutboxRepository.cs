using MultiTenantEcommerce.Domain.Common.Interfaces;

namespace MultiTenantEcommerce.Infrastructure.Outbox;
public interface IOutboxRepository : IRepository<OutboxEvent>
{
    public Task<List<OutboxEvent>> GetUnprocessedEvents(int batchSize);
}

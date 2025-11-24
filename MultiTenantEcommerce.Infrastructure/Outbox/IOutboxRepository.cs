using MultiTenantEcommerce.Domain.Enums;

namespace MultiTenantEcommerce.Infrastructure.Outbox;
public interface IOutboxRepository
{
    public Task<List<OutboxEvent>> GetUnprocessedEvents(EventPriority priority, int batchSize);
    public Task SaveChangesAsync();
    public Task UpdateAsync(OutboxEvent outboxEvent);
}

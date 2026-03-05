namespace MultiTenantEcommerce.Infrastructure.Shared.Outbox;

public interface IOutboxRepository
{
    public Task<List<OutboxEvent>> GetUnprocessedEvents(int batchSize);
    public Task SaveChangesAsync();
}
using Microsoft.EntityFrameworkCore;
using MultiTenantEcommerce.Domain.Enums;
using MultiTenantEcommerce.Infrastructure.Persistence.Context;

namespace MultiTenantEcommerce.Infrastructure.Outbox;
public class OutboxRepository : IOutboxRepository
{
    private readonly AppDbContext _appDbContext;

    public OutboxRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<List<OutboxEvent>> GetUnprocessedEvents(EventPriority priority, int batchSize)
    {
        return await _appDbContext.OutboxEvents
            .Where(x => x.ProcessedOn == null && x.Retries < 5 && x.Priority == priority)
            .OrderBy(x => x.OccurredOn)
            .Take(batchSize)
            .ToListAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _appDbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(OutboxEvent outboxEvent)
    {
        _appDbContext.OutboxEvents.Update(outboxEvent);
    }
}

using Microsoft.EntityFrameworkCore;
using MultiTenantEcommerce.Infrastructure.Persistence.Context;

namespace MultiTenantEcommerce.Infrastructure.Outbox;

public class OutboxRepository : IOutboxRepository
{
    private readonly AppDbContext _appDbContext;

    public OutboxRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<List<OutboxEvent>> GetUnprocessedEvents(int batchSize)
    {
        return await _appDbContext.OutboxEvents
            .Where(x => x.ProcessedOn == null && x.Retries < 5)
            .OrderBy(x => x.Priority)
            .ThenBy(x => x.OccurredOn)
            .Take(batchSize)
            .ToListAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _appDbContext.SaveChangesAsync();
    }
}
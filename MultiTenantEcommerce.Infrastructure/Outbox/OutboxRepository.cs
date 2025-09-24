using Microsoft.EntityFrameworkCore;
using MultiTenantEcommerce.Application.Common.Interfaces.Persistence;
using MultiTenantEcommerce.Infrastructure.Persistence.Context;
using MultiTenantEcommerce.Infrastructure.Persistence.Repositories;

namespace MultiTenantEcommerce.Infrastructure.Outbox;
public class OutboxRepository : Repository<OutboxEvent>, IOutboxRepository
{
    public OutboxRepository(AppDbContext appDbContext, ITenantContext tenantContext)
        : base(appDbContext, tenantContext)
    {
    }

    public async Task<List<OutboxEvent>> GetUnprocessedEvents(int batchSize)
    {
        return await _appDbContext.OutboxEvents
            .Where(x => x.ProcessedOn == null && x.Retries < 5)
            .OrderBy(x => x.OccurredOn)
            .Take(batchSize).ToListAsync();
    }
}

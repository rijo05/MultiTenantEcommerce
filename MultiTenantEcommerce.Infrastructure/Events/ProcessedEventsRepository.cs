using Microsoft.EntityFrameworkCore;
using MultiTenantEcommerce.Application.Common.Interfaces.Persistence;
using MultiTenantEcommerce.Infrastructure.Persistence.Context;
using MultiTenantEcommerce.Infrastructure.Persistence.Repositories;

namespace MultiTenantEcommerce.Infrastructure.Events;
public class ProcessedEventsRepository : Repository<ProcessedEvent>, IProcessedEventsRepository
{
    public ProcessedEventsRepository(AppDbContext appDbContext, ITenantContext tenantContext)
        : base(appDbContext, tenantContext)
    {
    }

    public async Task<bool> WasThisEventProcessedAlready(Guid eventId, string handlerName)
    {
        return await _appDbContext.ProcessedEvents
            .AnyAsync(x => x.EventId == eventId && x.HandlerName.ToLower() == handlerName.ToLower());
    }
}

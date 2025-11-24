using Microsoft.EntityFrameworkCore;
using MultiTenantEcommerce.Infrastructure.Persistence.Context;

namespace MultiTenantEcommerce.Infrastructure.Events;
public class ProcessedEventsRepository : IProcessedEventsRepository
{
    private readonly AppDbContext _appDbContext;
    public ProcessedEventsRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task AddAsync(ProcessedEvent processedEvent)
    {
        await _appDbContext.ProcessedEvents.AddAsync(processedEvent);
    }

    public async Task<bool> WasThisEventProcessedAlready(Guid eventId, string handlerName)
    {
        return await _appDbContext.ProcessedEvents
            .AnyAsync(x => x.EventId == eventId && x.HandlerName.ToLower() == handlerName.ToLower());
    }
}

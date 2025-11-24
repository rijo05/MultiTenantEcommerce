namespace MultiTenantEcommerce.Infrastructure.Events;
public interface IProcessedEventsRepository
{
    public Task<bool> WasThisEventProcessedAlready(Guid eventId, string handlerName);
    public Task AddAsync(ProcessedEvent processedEvent);
}

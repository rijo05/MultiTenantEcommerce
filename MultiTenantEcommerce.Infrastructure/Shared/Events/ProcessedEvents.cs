namespace MultiTenantEcommerce.Infrastructure.Shared.Events;

public class ProcessedEvent
{
    private ProcessedEvent()
    {
    }

    public ProcessedEvent(Guid eventId, string handlerName)
    {
        EventId = eventId;
        HandlerName = handlerName;
        ProcessedAt = DateTime.UtcNow;
    }

    public Guid EventId { get; set; }
    public string HandlerName { get; set; }
    public DateTime ProcessedAt { get; set; }
}
namespace MultiTenantEcommerce.Infrastructure.Events;
public class ProcessedEvent
{
    public Guid EventId { get; set; }
    public string HandlerName { get; set; }
    public DateTime ProcessedAt { get; set; }

    private ProcessedEvent() { }
    public ProcessedEvent(Guid eventId, string handlerName)
    {
        EventId = eventId;
        HandlerName = handlerName;
        ProcessedAt = DateTime.UtcNow;
    }
}

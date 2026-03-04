namespace MultiTenantEcommerce.Domain.Commerce.Shipping.Events;

public record ShipmentShippedEvent(
    Guid TenantId,
    Guid OrderId) : IDomainEvent, IEmailEvent
{
    public string TemplateName => "ShipmentShipped";
    public Guid EventId { get; init; } = Guid.NewGuid();
    public EventPriority EventPriority => EventPriority.NonCritical;
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
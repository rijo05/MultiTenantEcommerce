namespace MultiTenantEcommerce.Domain.Commerce.Shipping.Events;

public record ShipmentDeliveredEvent(
    Guid TenantId,
    Guid OrderId) : IDomainEvent, IEmailEvent
{
    public string TemplateName => "ShipmentDelivered";
    public Guid EventId { get; init; } = Guid.NewGuid();
    public EventPriority EventPriority => EventPriority.NonCritical;
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
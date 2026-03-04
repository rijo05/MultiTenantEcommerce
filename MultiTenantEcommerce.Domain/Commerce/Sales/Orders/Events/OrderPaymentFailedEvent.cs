namespace MultiTenantEcommerce.Domain.Commerce.Sales.Orders.Events;

public record OrderPaymentFailedEvent(
    Guid TenantId,
    Guid OrderId) : IDomainEvent, IEmailEvent
{
    public string TemplateName => "OrderPaymentFailed";
    public Guid EventId { get; init; } = Guid.NewGuid();
    public EventPriority EventPriority => EventPriority.NonCritical;
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
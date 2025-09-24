using MultiTenantEcommerce.Domain.Common.Events;
using MultiTenantEcommerce.Domain.Enums;

namespace MultiTenantEcommerce.Domain.Sales.Orders.Events;
public record OrderShippedEvent(
    Guid TenantId,
    Guid OrderId) : IDomainEvent, IEmailEvent
{
    public string TemplateName => "OrderShipped";
    public Guid EventId { get; init; } = Guid.NewGuid();
    public EventPriority EventPriority => EventPriority.NonCritical;
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

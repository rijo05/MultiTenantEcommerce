using MultiTenantEcommerce.Domain.Common.Events;
using MultiTenantEcommerce.Domain.Enums;

namespace MultiTenantEcommerce.Domain.Inventory.Events;
public record OutOfStockEvent(
    Guid TenantId,
    Guid ProductId) : IDomainEvent, IEmailEvent
{
    public string TemplateName => "OutOfStock";
    public Guid EventId { get; init; } = Guid.NewGuid();
    public EventPriority EventPriority => EventPriority.NonCritical;
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

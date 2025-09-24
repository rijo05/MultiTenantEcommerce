using MultiTenantEcommerce.Domain.Common.Events;
using MultiTenantEcommerce.Domain.Enums;

namespace MultiTenantEcommerce.Domain.Inventory.Events;
public record LowStockEvent(
    Guid TenantId,
    Guid ProductId,
    int CurrentQuantity,
    int MinimumQuantity) : IDomainEvent, IEmailEvent
{
    public string TemplateName => "LowStock";
    public Guid EventId { get; init; } = Guid.NewGuid();
    public EventPriority EventPriority => EventPriority.NonCritical;
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

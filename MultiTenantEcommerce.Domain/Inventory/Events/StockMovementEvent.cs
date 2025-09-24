using MultiTenantEcommerce.Domain.Common.Events;
using MultiTenantEcommerce.Domain.Enums;

namespace MultiTenantEcommerce.Domain.Inventory.Events;
public record StockMovementEvent(
    Guid TenantId,
    Guid ProductId,
    int Quantity,
    StockMovementReason StockMovementReason) : IDomainEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    public EventPriority EventPriority => EventPriority.NonCritical;
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
};

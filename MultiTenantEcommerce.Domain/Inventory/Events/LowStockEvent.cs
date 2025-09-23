using MultiTenantEcommerce.Domain.Common.Events;

namespace MultiTenantEcommerce.Domain.Inventory.Events;
public record LowStockEvent(
    Guid TenantId,
    Guid ProductId,
    int CurrentQuantity,
    int MinimumQuantity) : IDomainEvent, IEmailEvent
{
    public string TemplateName => "LowStock";
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

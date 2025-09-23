using MultiTenantEcommerce.Domain.Common.Events;

namespace MultiTenantEcommerce.Domain.Inventory.Events;
public record OutOfStockEvent(
    Guid TenantId,
    Guid ProductId) : IDomainEvent, IEmailEvent
{
    public string TemplateName => "OutOfStock";
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

using MultiTenantEcommerce.Shared.Domain.Events;

namespace MultiTenantEcommerce.Domain.Commerce.Sales.Orders.Events;

public record OrderPaidEvent(Guid TenantId, Guid OrderId) : IDomainEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
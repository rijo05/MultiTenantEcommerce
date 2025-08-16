using MultiTenantEcommerce.Domain.Common;

namespace MultiTenantEcommerce.Domain.Events.Orders;

public class ReserveStockEvent : IDomainEvent
{
    public Guid ReferenceId { get;}
    public int Quantity { get;}
    public DateTime OccurredOn { get; } = DateTime.UtcNow;

    public ReserveStockEvent(Guid referenceId, int quantity)
    {
        ReferenceId = referenceId;
        Quantity = quantity;
    }
}

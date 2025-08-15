using MultiTenantEcommerce.Domain.Common;

namespace MultiTenantEcommerce.Domain.Events.Orders;

public class ReleaseStockEvent : IDomainEvent
{
    public Guid ReferenceId { get;}
    public int Quantity { get;}
    public string Reason { get;}
    public DateTime OccurredOn { get;} = DateTime.UtcNow;

    public ReleaseStockEvent(Guid referenceId, int quantity, string reason)
    {
        ReferenceId = referenceId;
        Quantity = quantity;
        Reason = reason;
    }
}

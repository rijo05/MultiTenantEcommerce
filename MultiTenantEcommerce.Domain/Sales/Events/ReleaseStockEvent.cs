using MultiTenantEcommerce.Domain.Common.Events;

namespace MultiTenantEcommerce.Domain.Sales.Events;

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

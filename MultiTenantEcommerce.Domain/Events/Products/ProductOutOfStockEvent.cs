using MultiTenantEcommerce.Domain.Common;

namespace MultiTenantEcommerce.Domain.Events.Products;

public class ProductOutOfStockEvent : IDomainEvent
{
    public Guid ProductId { get; }
    public DateTime OccurredOn { get; } = DateTime.UtcNow;

    public ProductOutOfStockEvent(Guid productId)
    {
        ProductId = productId;
    }
}

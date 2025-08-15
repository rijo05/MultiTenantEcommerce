using MultiTenantEcommerce.Domain.Common;

namespace MultiTenantEcommerce.Domain.Events.Products;

public class ProductStockLowEvent : IDomainEvent
{
    public Guid ProductId { get; }
    public DateTime OccurredOn { get; } = DateTime.UtcNow;

    public ProductStockLowEvent(Guid productId)
    {
        ProductId = productId;
    }
}

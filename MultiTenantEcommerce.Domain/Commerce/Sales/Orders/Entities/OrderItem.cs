using MultiTenantEcommerce.Shared.Domain.Abstractions;
using MultiTenantEcommerce.Shared.Domain.ValueObjects;

namespace MultiTenantEcommerce.Domain.Commerce.Sales.Orders.Entities;

public class OrderItem : TenantBase
{
    private OrderItem()
    {
    }

    internal OrderItem(Guid orderId, Guid tenantId, Guid productId, string productName, Money unitPrice,
        PositiveQuantity quantity)
        : base(tenantId)
    {
        OrderId = orderId;
        ProductId = productId;
        ProductName = productName;
        UnitPrice = unitPrice;
        Quantity = quantity;
    }

    public Guid OrderId { get; private set; }
    public Guid ProductId { get; private set; }
    public string ProductName { get; private set; }
    public Money UnitPrice { get; }
    public PositiveQuantity Quantity { get; }
    public Money Total => new Money(Quantity.Value * UnitPrice.Value);
}
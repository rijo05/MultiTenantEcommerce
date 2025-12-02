using MultiTenantEcommerce.Domain.Common.Entities;
using MultiTenantEcommerce.Domain.ValueObjects;

namespace MultiTenantEcommerce.Domain.Sales.Orders.Entities;
public class OrderItem : TenantBase
{
    public Guid OrderId { get; private set; }
    public Guid ProductId { get; private set; }
    public string ProductName { get; private set; }
    public Money UnitPrice { get; private set; }
    public PositiveQuantity Quantity { get; private set; }
    public Money Total => new Money(Quantity.Value * UnitPrice.Value);

    private OrderItem() { }
    internal OrderItem(Guid orderId, Guid tenantId, Guid productId, string productName, Money unitPrice, PositiveQuantity quantity)
        : base(tenantId)
    {
        OrderId = orderId;
        ProductId = productId;
        ProductName = productName;
        UnitPrice = unitPrice;
        Quantity = quantity;
    }
}

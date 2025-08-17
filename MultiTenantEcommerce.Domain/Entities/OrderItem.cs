using MultiTenantEcommerce.Domain.Common.Guard;
using MultiTenantEcommerce.Domain.ValueObjects;

namespace MultiTenantEcommerce.Domain.Entities;
public class OrderItem : TenantBase
{
    public Guid OrderId { get; private set; }
    public Guid ProductId { get; private set; }
    public string Name { get; private set; }
    public Money UnitPrice { get; private set; }
    public int Quantity { get; private set; }
    public decimal Total => Quantity * UnitPrice.Value;

    private OrderItem() { }
    public OrderItem(Guid orderId, Guid tenantId, Product product, int quantity)
    {
        GuardCommon.AgainstNegativeOrZero(quantity, nameof(quantity));

        OrderId = orderId;
        ProductId = product.Id;
        Name = product.Name;
        UnitPrice = product.Price;
        Quantity = quantity;
    }
}

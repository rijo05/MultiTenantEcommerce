using MultiTenantEcommerce.Domain.Catalog.Entities;
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
    public decimal Total => Quantity.Value * UnitPrice.Value;

    private OrderItem() { }
    internal OrderItem(Guid orderId, Guid tenantId, Product product, PositiveQuantity quantity)
    {
        //primarykey deve mudar por causa do id de baseentity ##########
        OrderId = orderId;
        ProductId = product.Id;
        ProductName = product.Name;
        UnitPrice = product.Price;
        Quantity = quantity;
    }
}

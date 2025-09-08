using MultiTenantEcommerce.Domain.Catalog.Entities;
using MultiTenantEcommerce.Domain.Common.Entities;
using MultiTenantEcommerce.Domain.ValueObjects;

namespace MultiTenantEcommerce.Domain.Sales.ShoppingCart.Entities;
public class CartItem : TenantBase
{
    public Guid CartId { get; private set; }
    public Guid ProductId { get; private set; }
    public PositiveQuantity Quantity { get; private set; }
    public Product Product { get; private set; }

    private CartItem() { }
    internal CartItem(Product product, PositiveQuantity quantity)
    {
        Product = product;
        Quantity = quantity;
        ProductId = product.Id;
    }

    internal void DecreaseQuantity(PositiveQuantity quantity)
    {
        var remaining = Quantity.Value - quantity.Value;
        if (remaining < 0)
            throw new Exception("Invalid quantity");

        Quantity = new PositiveQuantity(remaining);
    }

    internal void IncreaseQuantity(PositiveQuantity quantity)
    {
        Quantity = new PositiveQuantity(Quantity.Value + quantity.Value);
    }
}

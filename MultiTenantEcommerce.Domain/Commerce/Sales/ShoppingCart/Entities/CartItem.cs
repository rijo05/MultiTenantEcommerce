using MultiTenantEcommerce.Shared.Domain.Abstractions;
using MultiTenantEcommerce.Shared.Domain.ValueObjects;

namespace MultiTenantEcommerce.Domain.Commerce.Sales.ShoppingCart.Entities;

public class CartItem : TenantBase
{
    public Guid CartId { get; private set; }
    public Guid ProductId { get; private set; }
    public PositiveQuantity Quantity { get; private set; }

    private CartItem()
    {
    }

    internal CartItem(Guid tenantId, Guid cartId, Guid productId, PositiveQuantity quantity)
        : base(tenantId)
    {
        Quantity = quantity;
        ProductId = productId;
        CartId = cartId;
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
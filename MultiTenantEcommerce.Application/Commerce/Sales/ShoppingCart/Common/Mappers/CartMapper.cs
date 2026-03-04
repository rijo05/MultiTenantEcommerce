using MultiTenantEcommerce.Application.Commerce.Catalog.Products.Common.Mappers;
using MultiTenantEcommerce.Application.Commerce.Sales.ShoppingCart.Common.DTOs;
using MultiTenantEcommerce.Domain.Commerce.Catalog.Entities;
using MultiTenantEcommerce.Domain.Commerce.Sales.ShoppingCart.Entities;
using MultiTenantEcommerce.Domain.Logistics.Inventory.Entities;

namespace MultiTenantEcommerce.Application.Commerce.Sales.ShoppingCart.Common.Mappers;

public static class CartMapper
{
    public static CartSummaryDTO ToSummaryDTO(this Cart cart)
    {
        return new CartSummaryDTO(cart.Id, cart.Items.Sum(x => x.Quantity.Value));
    }
}
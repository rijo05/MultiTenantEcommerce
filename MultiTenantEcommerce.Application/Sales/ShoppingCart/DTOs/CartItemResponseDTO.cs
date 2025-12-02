using MultiTenantEcommerce.Application.Catalog.Products.DTOs.Products;

namespace MultiTenantEcommerce.Application.Sales.ShoppingCart.DTOs;
public class CartItemResponseDTO
{
    public ProductResponseDTO Product { get; init; }
    public int Quantity { get; init; }
}

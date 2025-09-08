using MultiTenantEcommerce.Application.Sales.ShoppingCart.DTOs;
using MultiTenantEcommerce.Domain.Sales.ShoppingCart.Entities;

namespace MultiTenantEcommerce.Application.Sales.ShoppingCart.Mappers;
public class CartMapper
{
    public CartResponseDTO ToCartResponseDTO(Cart cart)
    {
        return new CartResponseDTO
        {
            CustomerId = cart.CustomerId,
            CreatedAt = cart.CreatedAt,
            UpdatedAt = cart.UpdatedAt,
            CartItems = ToCartItemResponseDTOList([.. cart.Items])
        };
    }

    public CartItemResponseDTO ToCartItemResponseDTO(CartItem item)
    {
        return new CartItemResponseDTO
        {
            ProductId = item.Product.Id,
            ProductName = item.Product.Name,
            Price = item.Product.Price.Value,
            Quantity = item.Quantity.Value,
        };
    }

    public List<CartItemResponseDTO> ToCartItemResponseDTOList(IEnumerable<CartItem> items)
    {
        return items.Select(x => ToCartItemResponseDTO(x)).ToList();
    }
}

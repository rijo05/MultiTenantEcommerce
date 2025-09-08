namespace MultiTenantEcommerce.Application.Sales.ShoppingCart.DTOs;
public class CartResponseDTO
{
    public Guid CustomerId { get; init; }
    public List<CartItemResponseDTO> CartItems { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}

namespace MultiTenantEcommerce.Application.Sales.ShoppingCart.DTOs;
public class CartItemResponseDTO
{
    public Guid ProductId { get; init; }
    public string ProductName { get; init; }
    public int Quantity { get; init; }
    public decimal Price { get; init; }

    //imagem...
}

namespace MultiTenantEcommerce.Application.Sales.ShoppingCart.DTOs;
public class ModifyQuantityDTO
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}

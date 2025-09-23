namespace MultiTenantEcommerce.Application.Sales.ShoppingCart.DTOs;
public record ModifyQuantityDTO(
    Guid ProductId,
    int Quantity);

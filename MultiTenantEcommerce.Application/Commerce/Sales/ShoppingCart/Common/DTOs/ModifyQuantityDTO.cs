namespace MultiTenantEcommerce.Application.Commerce.Sales.ShoppingCart.Common.DTOs;

public record ModifyQuantityDTO(
    Guid ProductId,
    int Quantity);
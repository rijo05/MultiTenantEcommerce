namespace MultiTenantEcommerce.Application.Commerce.Catalog.Products.Commands.Update;

public record UpdateProductDTO(
    string? Name,
    string? Description,
    decimal? Price,
    bool? IsActive,
    Guid? CategoryId);
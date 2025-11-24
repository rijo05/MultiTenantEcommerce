namespace MultiTenantEcommerce.Application.Catalog.Products.DTOs.Products;
public record UpdateProductDTO(
    string? Name,
    string? Description,
    decimal? Price,
    bool? IsActive,
    Guid? CategoryId);

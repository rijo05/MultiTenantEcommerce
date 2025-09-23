namespace MultiTenantEcommerce.Application.Catalog.Categories.DTOs;
public record UpdateCategoryDTO(
    string? Name,
    string? Description,
    bool? IsActive);

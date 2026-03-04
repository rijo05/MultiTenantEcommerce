namespace MultiTenantEcommerce.Application.Commerce.Catalog.Categories.Commands.Update;

public record UpdateCategoryDTO(
    string? Name,
    string? Description,
    bool? IsActive);
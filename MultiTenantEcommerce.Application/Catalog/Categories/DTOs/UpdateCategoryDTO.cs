namespace MultiTenantEcommerce.Application.Catalog.Categories.DTOs;
public class UpdateCategoryDTO
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public bool? IsActive { get; set; }
}

namespace MultiTenantEcommerce.Application.Catalog.Categories.DTOs;
public interface ICategoryDTO
{
    Guid Id { get; }
    string Name { get; }
    string? Description { get; }
}

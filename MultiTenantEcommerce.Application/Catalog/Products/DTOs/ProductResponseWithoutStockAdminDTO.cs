using MultiTenantEcommerce.Application.Catalog.Categories.DTOs;

namespace MultiTenantEcommerce.Application.Catalog.Products.DTOs;
public class ProductResponseWithoutStockAdminDTO
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string? Description { get; init; }
    public decimal Price { get; init; }
    public Guid CategoryId { get; init; }
    public CategoryResponseAdminDTO Category { get; init; }
    public bool IsActive { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}

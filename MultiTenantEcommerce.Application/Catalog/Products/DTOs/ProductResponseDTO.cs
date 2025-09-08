using MultiTenantEcommerce.Domain.Catalog.Entities;

namespace MultiTenantEcommerce.Application.Catalog.Products.DTOs;

public class ProductResponseDTO
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string? Description { get; init; }
    public decimal Price { get; init; }
    public Guid CategoryId { get; init; }
    public Category Category { get; init; }
    public bool IsActive { get; init; }
    public Dictionary<string, object> Links { get; init; }
}

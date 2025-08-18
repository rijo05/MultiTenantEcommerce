namespace MultiTenantEcommerce.Application.DTOs.Product;

public class ProductResponseDTO
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string? Description { get; init; }
    public decimal Price { get; init; }
    public Guid CategoryId { get; init; }
    public bool IsActive { get; init; }
    public Dictionary<string, object> Links { get; init; }
}

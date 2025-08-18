namespace MultiTenantEcommerce.Application.DTOs.Category;

public class CategoryResponseDTO
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string? Description { get; init; }
    public bool IsActive { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
    public Dictionary<string, object> Links { get; init; }
}

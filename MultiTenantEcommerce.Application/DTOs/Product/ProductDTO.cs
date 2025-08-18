namespace MultiTenantEcommerce.Application.DTOs.Product;

public class ProductDTO
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? SKU { get; set; }
    public decimal Price { get; set; }
    public Guid CategoryId { get; set; }
}

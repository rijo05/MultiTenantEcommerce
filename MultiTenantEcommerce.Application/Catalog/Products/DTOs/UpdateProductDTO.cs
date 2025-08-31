namespace MultiTenantEcommerce.Application.Catalog.Products.DTOs;
public class UpdateProductDTO
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal? Price { get; set; }
    public bool? IsActive { get; set; }
    public Guid? CategoryId { get; set; }
}

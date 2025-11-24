using MultiTenantEcommerce.Application.Catalog.Categories.DTOs;
using MultiTenantEcommerce.Application.Inventory.DTOs;
using MultiTenantEcommerce.Application.Catalog.Products.DTOs.Images;

namespace MultiTenantEcommerce.Application.Catalog.Products.DTOs.Products;
public class ProductResponseAdminDTO : IProductDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public Guid CategoryId { get; set; }
    public CategoryResponseAdminDTO Category { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public StockResponseAdminDTO Stock { get; set; }
    public List<IProductImageDTO> Images { get; set; }
}

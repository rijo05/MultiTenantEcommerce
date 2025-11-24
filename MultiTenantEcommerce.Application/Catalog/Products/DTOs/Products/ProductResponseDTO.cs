using MultiTenantEcommerce.Application.Catalog.Categories.DTOs;
using MultiTenantEcommerce.Application.Catalog.Products.DTOs.Images;
using MultiTenantEcommerce.Application.Inventory.DTOs;

namespace MultiTenantEcommerce.Application.Catalog.Products.DTOs.Products;

public class ProductResponseDTO : IProductDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public Guid CategoryId { get; set; }
    public CategoryResponseDTO Category { get; set; }
    public StockResponseDTO Stock { get; set; }
    public List<IProductImageDTO> Images { get; set; }
}

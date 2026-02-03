using MultiTenantEcommerce.Application.Catalog.Products.DTOs.Images;
using MultiTenantEcommerce.Application.Inventory.DTOs;

namespace MultiTenantEcommerce.Application.Catalog.Products.DTOs.Products;
public class ProductResponseAdminDTO : IProductDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public Guid CategoryId { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public StockResponseAdminDTO Stock { get; set; }
    public List<ProductImageResponseAdminDTO> Images { get; set; }


    IStockDTO IProductDTO.Stock => Stock;
    List<IProductImageDTO> IProductDTO.Images
    => Images.Cast<IProductImageDTO>().ToList();
}

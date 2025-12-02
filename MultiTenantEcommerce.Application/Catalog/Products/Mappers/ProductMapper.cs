using MultiTenantEcommerce.Application.Catalog.Categories.Mappers;
using MultiTenantEcommerce.Application.Catalog.Products.DTOs.Products;
using MultiTenantEcommerce.Application.Inventory.Mappers;
using MultiTenantEcommerce.Domain.Catalog.Entities;
using MultiTenantEcommerce.Domain.Inventory.Entities;

namespace MultiTenantEcommerce.Application.Catalog.Products.Mappers;

public class ProductMapper
{
    private readonly StockMapper _stockMapper;
    private readonly CategoryMapper _categoryMapper;
    private readonly ImageMapper _imageMapper;

    public ProductMapper(StockMapper stockMapper,
        CategoryMapper categoryMapper,
        ImageMapper imageMapper)
    {
        _stockMapper = stockMapper;
        _categoryMapper = categoryMapper;
        _imageMapper = imageMapper;
    }

    public ProductResponseDTO ToProductResponseDTO(Product product, Stock stock, Dictionary<string, string> signedUrls)
    {
        return new ProductResponseDTO
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price.Value,
            CategoryId = product.CategoryId,
            Stock = _stockMapper.ToStockResponseDTO(stock),
            Images = _imageMapper.ToProductImageResponseDTOList(product.Images, signedUrls)
        };
    }

    public List<ProductResponseDTO> ToProductResponseDTOList(IEnumerable<Product> products, IEnumerable<Stock> stocks, Dictionary<string, string> signedUrls)
    {
        var stockDict = stocks.ToDictionary(s => s.ProductId);

        return products.Select(p =>
        {
            var stock = stockDict.TryGetValue(p.Id, out var s) ? s : null;
            if (stock == null)
                throw new Exception($"Stock not found for product {p.Id}");

            return ToProductResponseDTO(p, stock, signedUrls);
        }).ToList();
    }


    public ProductResponseAdminDTO ToProductResponseAdminDTO(Product product, Stock stock, Dictionary<string, string> signedUrls)
    {
        return new ProductResponseAdminDTO
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price.Value,
            CategoryId = product.CategoryId,
            CreatedAt = product.CreatedAt,
            UpdatedAt = product.UpdatedAt,
            IsActive = product.IsActive,
            Stock = _stockMapper.ToStockResponseAdminDTO(stock),
            Images = _imageMapper.ToProductImageResponseAdminDTOList(product.Images, signedUrls)
        };
    }

    public List<ProductResponseAdminDTO> ToProductResponseAdminDTOList(IEnumerable<Product> products, IEnumerable<Stock> stocks, Dictionary<string, string> signedUrls)
    {
        var stockDict = stocks.ToDictionary(s => s.ProductId);

        return products.Select(p =>
        {
            var stock = stockDict.TryGetValue(p.Id, out var s) ? s : null;
            if (stock == null)
                throw new Exception($"Stock not found for product {p.Id}");

            return ToProductResponseAdminDTO(p, stock, signedUrls);
        }).ToList();
    }
}

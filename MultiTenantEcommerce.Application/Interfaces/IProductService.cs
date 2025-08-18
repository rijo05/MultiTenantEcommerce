using MultiTenantEcommerce.Application.DTOs.Product;

namespace MultiTenantEcommerce.Application.Interfaces;

public interface IProductService
{
    public Task<ProductResponseDTO?> GetProductByIdAsync(Guid id);
    public Task<List<ProductResponseDTO>> GetFilteredProductsAsync(ProductFilterDTO productFilterDTO);
    public Task<ProductResponseDTO?> GetProductBySKUAsync(string SKU);

    public Task<ProductResponseDTO> AddProductAsync(CreateProductDTO product);
    public Task DeleteProductAsync(Guid id);
    public Task<ProductResponseDTO> UpdateProductAsync(Guid id, UpdateProductDTO updatedProduct);
}

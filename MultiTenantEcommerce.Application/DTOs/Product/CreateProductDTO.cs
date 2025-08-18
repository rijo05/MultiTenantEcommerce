using MultiTenantEcommerce.Application.DTOs.Stock;

namespace MultiTenantEcommerce.Application.DTOs.Product;
public class CreateProductDTO
{
    public ProductDTO ProductDTO { get; set; }
    public CreateStockDTO stockDTO { get; set; }
}

using MultiTenantEcommerce.Application.Catalog.Products.DTOs.Images;
using MultiTenantEcommerce.Application.Common.DTOs;

namespace MultiTenantEcommerce.Application.Catalog.Products.DTOs.Products;

public interface IProductDTO
{
    Guid Id { get; }
    string Name { get; }
    string? Description { get; }
    decimal Price { get; }
    public Guid CategoryId { get; }
    public List<IProductImageDTO> Images { get; }

}
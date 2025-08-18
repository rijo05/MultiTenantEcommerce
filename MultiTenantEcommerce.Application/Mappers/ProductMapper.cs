using MultiTenantEcommerce.Application.DTOs.Product;
using MultiTenantEcommerce.Application.Services;
using MultiTenantEcommerce.Domain.Entities;

namespace MultiTenantEcommerce.Application.Mappers;

public class ProductMapper
{
    private readonly HateoasLinkService _hateoasLinkService;

    public ProductMapper(HateoasLinkService hateoasLinkService)
    {
        _hateoasLinkService = hateoasLinkService;
    }

    public ProductResponseDTO ToProductResponseDTO(Product product)
    {
        return new ProductResponseDTO
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price.Value,
            CategoryId = product.CategoryId,
            IsActive = product.IsActive,
            //Links = GenerateLinks(product)
        };
    }

    public List<ProductResponseDTO> ToProductResponseDTOList(List<Product> products)
    {
        return products.Select(x => ToProductResponseDTO(x)).ToList();
    }


    //private Dictionary<string, object> GenerateLinks(Product product)
    //{
    //    return _hateoasLinkService.GenerateLinksCRUD(
    //                product.Id,
    //                "products",
    //                "GetById",
    //                "Update",
    //                "Delete"
    //    );
    //}

}

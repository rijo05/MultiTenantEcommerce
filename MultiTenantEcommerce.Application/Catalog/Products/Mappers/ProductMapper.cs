using MultiTenantEcommerce.Application.Catalog.Products.DTOs;
using MultiTenantEcommerce.Application.Common.Helpers;
using MultiTenantEcommerce.Domain.Catalog.Entities;

namespace MultiTenantEcommerce.Application.Catalog.Products.Mappers;

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
            Category = product.Category,
            IsActive = product.IsActive,
            //Links = GenerateLinks(product)
        };
    }

    public List<ProductResponseDTO> ToProductResponseDTOList(IEnumerable<Product> products)
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

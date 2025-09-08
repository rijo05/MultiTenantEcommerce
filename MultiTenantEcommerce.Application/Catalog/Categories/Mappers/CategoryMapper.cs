using MultiTenantEcommerce.Application.Catalog.Categories.DTOs;
using MultiTenantEcommerce.Application.Common.Helpers;
using MultiTenantEcommerce.Domain.Catalog.Entities;

namespace MultiTenantEcommerce.Application.Catalog.Categories.Mappers;

public class CategoryMapper
{
    private readonly HateoasLinkService _hateoasLinkService;

    public CategoryMapper(HateoasLinkService hateoasLinkService)
    {
        _hateoasLinkService = hateoasLinkService;
    }

    public CategoryResponseDTO ToCategoryResponseDTO(Category category)
    {
        return new CategoryResponseDTO
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            IsActive = category.IsActive,
            CreatedAt = category.CreatedAt,
            UpdatedAt = category.UpdatedAt,
            //Links = GenerateLinks(category)
        };
    }

    public List<CategoryResponseDTO> ToCategoryResponseDTOList(IEnumerable<Category> categories)
    {
        return categories.Select(x => ToCategoryResponseDTO(x)).ToList();
    }


    //private Dictionary<string, object> GenerateLinks(Category category)
    //{
    //    return _hateoasLinkService.GenerateLinksCRUD(
    //                category.Id,
    //                "categories",
    //                "GetById",
    //                "Update",
    //                "Delete"
    //    );
    //}
}

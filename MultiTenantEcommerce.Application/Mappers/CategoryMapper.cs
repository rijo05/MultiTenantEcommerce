using MultiTenantEcommerce.Application.DTOs.Category;
using MultiTenantEcommerce.Application.Services;
using MultiTenantEcommerce.Domain.Entities;

namespace MultiTenantEcommerce.Application.Mappers;

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

    public List<CategoryResponseDTO> ToCategoryResponseDTOList(List<Category> categories)
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

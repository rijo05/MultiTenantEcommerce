using MultiTenantEcommerce.Application.Catalog.Categories.DTOs;
using MultiTenantEcommerce.Application.Common.Helpers.Services;
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
        };
    }

    public List<CategoryResponseDTO> ToCategoryResponseDTOList(IEnumerable<Category> categories)
    {
        return categories.Select(x => ToCategoryResponseDTO(x)).ToList();
    }



    public CategoryResponseAdminDTO ToCategoryResponseAdminDTO(Category category)
    {
        return new CategoryResponseAdminDTO
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            IsActive = category.IsActive,
            CreatedAt = category.CreatedAt,
            UpdatedAt = category.UpdatedAt,
        };
    }

    public List<CategoryResponseAdminDTO> ToCategoryResponseAdminDTOList(IEnumerable<Category> categories)
    {
        return categories.Select(x => ToCategoryResponseAdminDTO(x)).ToList();
    }
}

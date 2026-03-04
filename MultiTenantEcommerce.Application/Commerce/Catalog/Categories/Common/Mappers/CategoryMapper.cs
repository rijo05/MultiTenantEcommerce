using MultiTenantEcommerce.Application.Commerce.Catalog.Categories.Common.DTOs;
using MultiTenantEcommerce.Application.Commerce.Catalog.Products.Common.DTOs.Products;
using MultiTenantEcommerce.Application.Common.Helpers.Services;
using MultiTenantEcommerce.Domain.Commerce.Catalog.Entities;
using MultiTenantEcommerce.Shared.Application;
using MultiTenantEcommerce.Shared.Integration.DTOs;

namespace MultiTenantEcommerce.Application.Commerce.Catalog.Categories.Common.Mappers;

public static class CategoryMapper
{
    public static CategoryResponseDTO ToDTO(this Category category)
    {
        return new CategoryResponseDTO(
            category.Id,
            category.Name,
            category.Description
        );
    }

    public static List<CategoryResponseDTO> ToDTOList(this IEnumerable<Category> categories)
    {
        return categories.Select(x => ToDTO(x)).ToList();
    }

    public static CategoryResponseAdminDTO ToDTOAdmin(this Category category)
    {
        return new CategoryResponseAdminDTO(
            category.Id,
            category.Name,
            category.Description,
            category.IsActive,
            category.CreatedAt,
            category.UpdatedAt
        );
    }

    public static List<CategoryResponseAdminDTO> ToCategoryResponseAdminDTOList(this IEnumerable<Category> categories)
    {
        return categories.Select(x => ToDTOAdmin(x)).ToList();
    }

    public static PaginatedList<CategoryResponseAdminDTO> ToPaginatedDTOAdmin(this PaginatedList<Category> paginatedCategory)
    {
        var dtoList = ToCategoryResponseAdminDTOList(paginatedCategory.Items);

        return new PaginatedList<CategoryResponseAdminDTO>(
            dtoList,
            paginatedCategory.TotalCount,
            paginatedCategory.Page,
            paginatedCategory.PageSize
        );
    }
}
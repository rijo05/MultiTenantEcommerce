using MultiTenantEcommerce.Application.Commerce.Catalog.Categories.Common.DTOs;
using MultiTenantEcommerce.Application.Commerce.Catalog.Categories.Common.Mappers;
using MultiTenantEcommerce.Domain.Commerce.Catalog.Interfaces;
using MultiTenantEcommerce.Shared.Application;
using MultiTenantEcommerce.Shared.Application.CQRS;

namespace MultiTenantEcommerce.Application.Commerce.Catalog.Categories.Queries.GetFilteredAdmin;

public class GetFilteredCategoriesAdminQueryHandler : IQueryHandler<GetFilteredCategoriesAdminQuery, PaginatedList<CategoryResponseAdminDTO>>
{
    private readonly ICategoryRepository _categoryRepository;

    public GetFilteredCategoriesAdminQueryHandler(
        ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<PaginatedList<CategoryResponseAdminDTO>> Handle(GetFilteredCategoriesAdminQuery request, CancellationToken cancellationToken)
    {
        var categories = await _categoryRepository.GetFilteredAsync(
            request.Name,
            request.Description,
            request.IsActive,
            request.Page,
            request.PageSize,
            request.Sort);

        return categories.ToPaginatedDTOAdmin();
    }
}
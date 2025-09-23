using MultiTenantEcommerce.Application.Catalog.Categories.DTOs;
using MultiTenantEcommerce.Application.Catalog.Categories.Mappers;
using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Domain.Catalog.Interfaces;

namespace MultiTenantEcommerce.Application.Catalog.Categories.Queries.GetFiltered;
public class GetFilteredCategoriesQueryHandler : IQueryHandler<GetFilteredCategoriesQuery, List<ICategoryDTO>>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly CategoryMapper _categoryMapper;

    public GetFilteredCategoriesQueryHandler(
        ICategoryRepository categoryRepository,
        CategoryMapper categoryMapper)
    {
        _categoryRepository = categoryRepository;
        _categoryMapper = categoryMapper;
    }

    public async Task<List<ICategoryDTO>> Handle(GetFilteredCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await _categoryRepository.GetFilteredAsync(
            request.Name,
            request.Description,
            request.IsActive,
            request.Page,
            request.PageSize,
            request.Sort);

        return request.IsAdmin
            ? _categoryMapper.ToCategoryResponseAdminDTOList(categories).Cast<ICategoryDTO>().ToList()
            : _categoryMapper.ToCategoryResponseDTOList(categories).Cast<ICategoryDTO>().ToList();
    }
}

using MultiTenantEcommerce.Application.Catalog.Categories.DTOs;
using MultiTenantEcommerce.Application.Catalog.Categories.Mappers;
using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Domain.Catalog.Interfaces;

namespace MultiTenantEcommerce.Application.Catalog.Categories.Queries.GetById;
public class GetCategoryByIdQueryHandler : IQueryHandler<GetCategoryByIdQuery, ICategoryDTO>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly CategoryMapper _categoryMapper;

    public GetCategoryByIdQueryHandler(
        ICategoryRepository categoryRepository,
        CategoryMapper categoryMapper)
    {
        _categoryRepository = categoryRepository;
        _categoryMapper = categoryMapper;
    }

    public async Task<ICategoryDTO> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(request.CategoryId)
            ?? throw new Exception("Category doesnt exist.");

        return request.IsAdmin
            ? _categoryMapper.ToCategoryResponseAdminDTO(category)
            : _categoryMapper.ToCategoryResponseDTO(category);
    }
}

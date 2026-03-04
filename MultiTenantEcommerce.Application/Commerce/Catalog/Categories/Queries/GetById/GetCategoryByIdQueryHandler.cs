using MultiTenantEcommerce.Application.Commerce.Catalog.Categories.Common.DTOs;
using MultiTenantEcommerce.Application.Commerce.Catalog.Categories.Common.Mappers;
using MultiTenantEcommerce.Domain.Commerce.Catalog.Interfaces;
using MultiTenantEcommerce.Shared.Application.CQRS;

namespace MultiTenantEcommerce.Application.Commerce.Catalog.Categories.Queries.GetById;

public class GetCategoryByIdQueryHandler : IQueryHandler<GetCategoryByIdQuery, CategoryResponseDTO>
{
    private readonly ICategoryRepository _categoryRepository;

    public GetCategoryByIdQueryHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<CategoryResponseDTO> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(request.CategoryId);

        if (category == null || !category.IsActive)
            throw new Exception("Category doesnt exist");

        return category.ToDTO();
    }
}
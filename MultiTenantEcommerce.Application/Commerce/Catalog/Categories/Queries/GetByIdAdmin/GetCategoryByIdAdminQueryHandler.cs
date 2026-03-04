using MultiTenantEcommerce.Application.Commerce.Catalog.Categories.Common.DTOs;
using MultiTenantEcommerce.Application.Commerce.Catalog.Categories.Common.Mappers;
using MultiTenantEcommerce.Domain.Commerce.Catalog.Interfaces;
using MultiTenantEcommerce.Shared.Application.CQRS;

namespace MultiTenantEcommerce.Application.Commerce.Catalog.Categories.Queries.GetByIdAdmin;

public class GetCategoryByIdAdminQueryHandler : IQueryHandler<GetCategoryByIdAdminQuery, CategoryResponseAdminDTO>
{
    private readonly ICategoryRepository _categoryRepository;

    public GetCategoryByIdAdminQueryHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;

    }

    public async Task<CategoryResponseAdminDTO> Handle(GetCategoryByIdAdminQuery request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(request.CategoryId)
                       ?? throw new Exception("Category doesnt exist.");

        return category.ToDTOAdmin();
    }
}
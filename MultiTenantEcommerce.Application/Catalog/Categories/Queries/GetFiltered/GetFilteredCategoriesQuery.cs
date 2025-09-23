using MultiTenantEcommerce.Application.Catalog.Categories.DTOs;
using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Domain.Enums;

namespace MultiTenantEcommerce.Application.Catalog.Categories.Queries.GetFiltered;
public record GetFilteredCategoriesQuery(
    string? Name,
    string? Description,
    bool? IsActive,
    bool IsAdmin,
    int Page = 1,
    int PageSize = 20,
    SortOptions Sort = SortOptions.TimeDesc) : IQuery<List<ICategoryDTO>>;

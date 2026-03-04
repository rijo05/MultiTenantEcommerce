using MultiTenantEcommerce.Application.Commerce.Catalog.Categories.Common.DTOs;
using MultiTenantEcommerce.Shared.Application.CQRS;

namespace MultiTenantEcommerce.Application.Commerce.Catalog.Categories.Queries.GetByIdAdmin;

public record GetCategoryByIdAdminQuery(
    Guid CategoryId) : IQuery<CategoryResponseAdminDTO>;
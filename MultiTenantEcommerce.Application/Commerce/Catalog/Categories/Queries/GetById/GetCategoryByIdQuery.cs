using MultiTenantEcommerce.Application.Commerce.Catalog.Categories.Common.DTOs;
using MultiTenantEcommerce.Shared.Application.CQRS;

namespace MultiTenantEcommerce.Application.Commerce.Catalog.Categories.Queries.GetById;

public record GetCategoryByIdQuery(
    Guid CategoryId) : IQuery<CategoryResponseDTO>;
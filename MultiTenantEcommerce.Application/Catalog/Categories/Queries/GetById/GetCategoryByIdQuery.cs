using MultiTenantEcommerce.Application.Catalog.Categories.DTOs;
using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;

namespace MultiTenantEcommerce.Application.Catalog.Categories.Queries.GetById;
public record GetCategoryByIdQuery(
    Guid CategoryId) : IQuery<CategoryResponseDTO>;

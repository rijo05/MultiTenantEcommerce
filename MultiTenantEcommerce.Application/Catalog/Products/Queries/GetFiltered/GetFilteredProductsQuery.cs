using MultiTenantEcommerce.Application.Catalog.Products.DTOs;
using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Domain.Enums;

namespace MultiTenantEcommerce.Application.Catalog.Products.Queries.GetFiltered;
public record GetFilteredProductsQuery(
    string? CategoryName,
    Guid? CategoryId,
    string? Name,
    decimal? MinPrice,
    decimal? MaxPrice,
    bool? IsActive,
    int Page = 1,
    int PageSize = 20,
    SortOptions Sort = SortOptions.TimeDesc) : IQuery<List<ProductResponseDTO>>;

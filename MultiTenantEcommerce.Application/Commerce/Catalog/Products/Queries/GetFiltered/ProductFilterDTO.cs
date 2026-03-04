using MultiTenantEcommerce.Shared.Application.CQRS;

namespace MultiTenantEcommerce.Application.Commerce.Catalog.Products.Queries.GetFiltered;

public record ProductFilterDTO(
    string? CategoryName,
    Guid? CategoryId,
    string? Name,
    decimal? MinPrice,
    decimal? MaxPrice,
    int Page = 1,
    int PageSize = 20,
    SortOptions Sort = SortOptions.TimeDesc);
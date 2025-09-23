using MultiTenantEcommerce.Domain.Enums;

namespace MultiTenantEcommerce.Application.Catalog.Products.DTOs;
public record ProductFilterDTO(
    string? CategoryName,
    Guid? CategoryId,
    string? Name,
    decimal? MinPrice,
    decimal? MaxPrice,
    int Page = 1,
    int PageSize = 20,
    SortOptions Sort = SortOptions.TimeDesc);
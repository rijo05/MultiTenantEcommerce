using MultiTenantEcommerce.Domain.Enums;

namespace MultiTenantEcommerce.Application.Catalog.Products.DTOs.Products;
public record ProductFilterAdminDTO(
    string? CategoryName,
    Guid? CategoryId,
    string? Name,
    decimal? MinPrice,
    decimal? MaxPrice,
    bool? IsActive,
    int Page = 1,
    int PageSize = 20,
    SortOptions Sort = SortOptions.TimeDesc);

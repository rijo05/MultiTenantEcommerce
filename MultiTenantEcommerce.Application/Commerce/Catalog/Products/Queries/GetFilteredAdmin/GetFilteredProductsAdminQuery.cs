using MultiTenantEcommerce.Application.Commerce.Catalog.Products.Common.DTOs.Products;
using MultiTenantEcommerce.Domain.Commerce.Catalog.Enums;
using MultiTenantEcommerce.Shared.Application.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiTenantEcommerce.Shared.Application;

namespace MultiTenantEcommerce.Application.Commerce.Catalog.Products.Queries.GetFilteredAdmin;
public record GetFilteredProductsAdminQuery(
    Guid? CategoryId,
    string? Name,
    decimal? MinPrice,
    decimal? MaxPrice,
    bool? IsActive,
    List<StockStatus>? StockStatuses,
    int Page = 1,
    int PageSize = 20,
    SortOptions Sort = SortOptions.TimeDesc) : IQuery<PaginatedList<ProductResponseAdminDTO>>;

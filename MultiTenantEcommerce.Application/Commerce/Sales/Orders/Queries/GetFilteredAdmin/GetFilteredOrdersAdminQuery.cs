using MultiTenantEcommerce.Shared.Application;
using MultiTenantEcommerce.Shared.Application.CQRS;

namespace MultiTenantEcommerce.Application.Commerce.Sales.Orders.Queries.GetFilteredAdmin;

public record GetFilteredOrdersAdminQuery(
    Guid? CustomerId,
    string? Status,
    DateTime? MinDate,
    DateTime? MaxDate,
    decimal? MinPrice,
    decimal? MaxPrice,
    int Page = 1,
    int PageSize = 20,
    SortOptions SortOptions = SortOptions.TimeDesc) : IQuery<PaginatedList<OrderSummaryAdminDTO>>;
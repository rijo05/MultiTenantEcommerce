using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Sales.Orders.DTOs;
using MultiTenantEcommerce.Domain.Enums;

namespace MultiTenantEcommerce.Application.Sales.Orders.Queries.GetFiltered;
public record GetFilteredOrdersQuery(
    Guid? CustomerId,
    string? Status,
    DateTime? MinDate,
    DateTime? MaxDate,
    decimal? MinPrice,
    decimal? MaxPrice,
    int Page = 1,
    int PageSize = 20,
    SortOptions SortOptions = SortOptions.TimeDesc) : IQuery<List<OrderResponseDTO>>;

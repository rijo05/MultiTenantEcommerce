using MultiTenantEcommerce.Application.Commerce.Customers.Common.DTOs;
using MultiTenantEcommerce.Shared.Application;
using MultiTenantEcommerce.Shared.Application.CQRS;

namespace MultiTenantEcommerce.Application.Commerce.Customers.Queries.GetFiltered;

public record GetFilteredCustomerQuery(
    string? Name,
    string? Email,
    bool? IsActive,
    int Page = 1,
    int PageSize = 20,
    SortOptions Sort = SortOptions.TimeDesc) : IQuery<PaginatedList<CustomerResponseDTO>>;
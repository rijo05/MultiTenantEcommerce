using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Users.Customers.DTOs;
using MultiTenantEcommerce.Domain.Enums;

namespace MultiTenantEcommerce.Application.Users.Customers.Queries.GetFiltered;
public record GetFilteredCustomerQuery(
    string? Name,
    string? PhoneNumber,
    string? Email,
    bool? IsActive,
    int Page = 1,
    int PageSize = 20,
    SortOptions Sort = SortOptions.TimeDesc) : IQuery<List<CustomerResponseDTO>>;

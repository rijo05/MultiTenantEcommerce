using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Users.Employees.DTOs;
using MultiTenantEcommerce.Domain.Enums;

namespace MultiTenantEcommerce.Application.Users.Employees.Queries.GetFiltered;
public record GetFilteredEmployeesQuery(
    string? Name,
    string? Role,
    string? Email,
    bool? IsActive,
    int Page = 1,
    int PageSize = 20,
    SortOptions Sort = SortOptions.TimeDesc) : IQuery<List<EmployeeResponseDTO>>;

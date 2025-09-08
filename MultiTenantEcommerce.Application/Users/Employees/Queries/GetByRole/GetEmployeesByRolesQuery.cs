using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Users.Employees.DTOs;

namespace MultiTenantEcommerce.Application.Users.Employees.Queries.GetByRole;
public record GetEmployeesByRolesQuery(
    Guid roleId) : IQuery<List<EmployeeResponseDTO>>;

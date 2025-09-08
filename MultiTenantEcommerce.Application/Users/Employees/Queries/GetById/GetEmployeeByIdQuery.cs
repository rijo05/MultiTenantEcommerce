using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Users.Employees.DTOs;

namespace MultiTenantEcommerce.Application.Users.Employees.Queries.GetById;
public record GetEmployeeByIdQuery(
    Guid EmployeeId) : IQuery<EmployeeResponseDTO>;

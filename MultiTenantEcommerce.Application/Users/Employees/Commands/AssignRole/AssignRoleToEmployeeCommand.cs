using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Users.Employees.DTOs;

namespace MultiTenantEcommerce.Application.Users.Employees.Commands.AssignRole;
public record AssignRoleToEmployeeCommand(
    Guid employeeId,
    List<Guid> roles) : ICommand<EmployeeResponseDTO>;

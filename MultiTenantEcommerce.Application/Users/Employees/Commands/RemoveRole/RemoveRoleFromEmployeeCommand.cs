using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Users.Employees.DTOs;

namespace MultiTenantEcommerce.Application.Users.Employees.Commands.RemoveRole;
public record RemoveRoleFromEmployeeCommand(
    Guid employeeId,
    List<Guid> roles) : ICommand<EmployeeResponseDTO>;

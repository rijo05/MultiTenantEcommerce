using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Users.Employees.DTOs;

namespace MultiTenantEcommerce.Application.Users.Employees.Commands.Delete;
public record UpdateEmployeeCommand(
    Guid Id,
    string? Name,
    string? Email,
    string? Password,
    string? Role,
    bool? IsActive) : ICommand<EmployeeResponseDTO>;

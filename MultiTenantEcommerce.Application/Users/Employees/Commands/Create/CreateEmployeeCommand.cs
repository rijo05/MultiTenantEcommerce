using MediatR;
using MultiTenantEcommerce.Application.Common.Interfaces;
using MultiTenantEcommerce.Application.Users.Employees.DTOs;
using System.Windows.Input;

namespace MultiTenantEcommerce.Application.Users.Employees.Commands.Create;
public record CreateEmployeeCommand(
    string Name,
    string Email,
    string Password,
    string Role = "Employee") : ICommand<EmployeeResponseDTO>;

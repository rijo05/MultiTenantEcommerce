using MultiTenantEcommerce.Application.Common.Interfaces;
using MultiTenantEcommerce.Application.Users.Employees.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MultiTenantEcommerce.Application.Users.Employees.Commands.Delete;
public record UpdateEmployeeCommand(
    Guid Id,
    string? Name,
    string? Email,
    string? Password,
    string? Role,
    bool? IsActive) : ICommand<EmployeeResponseDTO>;

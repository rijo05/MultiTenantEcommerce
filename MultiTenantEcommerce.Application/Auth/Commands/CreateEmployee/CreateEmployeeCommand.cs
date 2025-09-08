using MultiTenantEcommerce.Application.Auth.DTOs;
using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;

namespace MultiTenantEcommerce.Application.Auth.Commands.CreateEmployee;
public record CreateEmployeeCommand(
    string Name,
    string Email,
    List<Guid> RolesId) : ICommand<AuthEmployeeResponseDTO>;

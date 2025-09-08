using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Users.Permissions.DTOs;

namespace MultiTenantEcommerce.Application.Users.Permissions.Commands.CreateRole;
public record CreateRoleCommand(
    string Name,
    string Description,
    List<Guid> permissions) : ICommand<RoleResponseDTO>;

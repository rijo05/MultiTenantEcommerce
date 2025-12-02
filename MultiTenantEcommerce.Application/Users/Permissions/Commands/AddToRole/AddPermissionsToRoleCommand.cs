using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Users.Permissions.DTOs;

namespace MultiTenantEcommerce.Application.Users.Permissions.Commands.AddToRole;
public record AddPermissionsToRoleCommand(
    Guid RoleId,
    List<Guid> Permissions) : ICommand<RoleDetailResponseDTO>;

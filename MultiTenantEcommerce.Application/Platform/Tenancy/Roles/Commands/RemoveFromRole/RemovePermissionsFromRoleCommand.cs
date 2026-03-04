using MultiTenantEcommerce.Application.Platform.Tenancy.Roles.Common.DTOs;

namespace MultiTenantEcommerce.Application.Platform.Tenancy.Roles.Commands.RemoveFromRole;

public record RemovePermissionsFromRoleCommand(
    Guid RoleId,
    List<Guid> Permissions) : ICommand<RoleDetailResponseDTO>;
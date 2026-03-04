using MultiTenantEcommerce.Application.Platform.Tenancy.Roles.Common.DTOs;

namespace MultiTenantEcommerce.Application.Platform.Tenancy.Roles.Commands.AddToRole;

public record AddPermissionsToRoleCommand(
    Guid RoleId,
    List<Guid> Permissions) : ICommand<RoleDetailResponseDTO>;
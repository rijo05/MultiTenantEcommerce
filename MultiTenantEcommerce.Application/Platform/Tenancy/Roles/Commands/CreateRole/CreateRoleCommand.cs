using MultiTenantEcommerce.Application.Platform.Tenancy.Roles.Common.DTOs;

namespace MultiTenantEcommerce.Application.Platform.Tenancy.Roles.Commands.CreateRole;

public record CreateRoleCommand(
    string Name,
    string Description,
    List<Guid> Permissions) : ICommand<RoleDetailResponseDTO>;
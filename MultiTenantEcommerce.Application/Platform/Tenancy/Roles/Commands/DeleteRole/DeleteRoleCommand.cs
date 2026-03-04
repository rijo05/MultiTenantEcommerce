using MediatR;

namespace MultiTenantEcommerce.Application.Platform.Tenancy.Roles.Commands.DeleteRole;

public record DeleteRoleCommand(
    Guid RoleId) : ICommand<Unit>;
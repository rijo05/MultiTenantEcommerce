using MediatR;
using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;

namespace MultiTenantEcommerce.Application.Users.Permissions.Commands.DeleteRole;
public record DeleteRoleCommand(
    Guid RoleId) : ICommand<Unit>;

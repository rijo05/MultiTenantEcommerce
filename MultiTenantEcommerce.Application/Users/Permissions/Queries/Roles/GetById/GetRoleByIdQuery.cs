using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Users.Permissions.DTOs;

namespace MultiTenantEcommerce.Application.Users.Permissions.Queries.Roles.GetById;
public record GetRoleByIdQuery(
    Guid roleId) : IQuery<RoleResponseDTO>;

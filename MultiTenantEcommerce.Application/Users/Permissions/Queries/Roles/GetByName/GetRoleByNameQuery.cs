using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Users.Permissions.DTOs;

namespace MultiTenantEcommerce.Application.Users.Permissions.Queries.Roles.GetByName;
public record GetRoleByNameQuery(
    string Name) : IQuery<RoleDetailResponseDTO>;


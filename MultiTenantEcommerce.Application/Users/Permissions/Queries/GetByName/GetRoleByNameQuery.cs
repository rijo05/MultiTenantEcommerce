using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Users.Permissions.DTOs;

namespace MultiTenantEcommerce.Application.Users.Permissions.Queries.GetByName;
public record GetRoleByNameQuery(
    string Name) : IQuery<RoleResponseDTO>;


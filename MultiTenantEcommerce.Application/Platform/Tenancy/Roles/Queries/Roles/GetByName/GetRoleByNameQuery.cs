using MultiTenantEcommerce.Application.Platform.Tenancy.Roles.Common.DTOs;

namespace MultiTenantEcommerce.Application.Platform.Tenancy.Roles.Queries.Roles.GetByName;

public record GetRoleByNameQuery(
    string Name) : IQuery<RoleDetailResponseDTO>;
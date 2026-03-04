using MultiTenantEcommerce.Application.Platform.Tenancy.Roles.Common.DTOs;

namespace MultiTenantEcommerce.Application.Platform.Tenancy.Roles.Queries.Roles.GetById;

public record GetRoleByIdQuery(
    Guid RoleId) : IQuery<RoleDetailResponseDTO>;
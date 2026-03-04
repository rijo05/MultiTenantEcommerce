using MultiTenantEcommerce.Application.Platform.Tenancy.Roles.Common.DTOs;

namespace MultiTenantEcommerce.Application.Platform.Tenancy.Roles.Queries.Permissions.GetByAction;

public record GetPermissionByActionQuery(
    string Action) : IQuery<List<PermissionResponseDTO>>;
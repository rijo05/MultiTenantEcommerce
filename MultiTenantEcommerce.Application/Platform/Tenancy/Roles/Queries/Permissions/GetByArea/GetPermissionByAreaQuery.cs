using MultiTenantEcommerce.Application.Platform.Tenancy.Roles.Common.DTOs;

namespace MultiTenantEcommerce.Application.Platform.Tenancy.Roles.Queries.Permissions.GetByArea;

public record GetPermissionByAreaQuery(
    string Area) : IQuery<List<PermissionResponseDTO>>;
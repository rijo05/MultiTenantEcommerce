using MultiTenantEcommerce.Application.Platform.Tenancy.Roles.Common.DTOs;

namespace MultiTenantEcommerce.Application.Platform.Tenancy.Roles.Queries.Permissions.GetAll;

public record GetAllPermissionsQuery : IQuery<List<PermissionResponseDTO>>;
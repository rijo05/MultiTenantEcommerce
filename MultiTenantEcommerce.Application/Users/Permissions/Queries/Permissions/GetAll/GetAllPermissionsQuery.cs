using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Users.Permissions.DTOs;

namespace MultiTenantEcommerce.Application.Users.Permissions.Queries.Permissions.GetAll;
public record GetAllPermissionsQuery() : IQuery<List<PermissionResponseDTO>>;

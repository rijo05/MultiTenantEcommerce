using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Users.Permissions.DTOs;

namespace MultiTenantEcommerce.Application.Users.Permissions.Queries.Permissions.GetByAction;
public record GetPermissionByActionQuery(
    string Action) : IQuery<List<PermissionResponseDTO>>;

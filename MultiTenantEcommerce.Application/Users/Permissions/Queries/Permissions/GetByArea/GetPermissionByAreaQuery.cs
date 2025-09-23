using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Users.Permissions.DTOs;

namespace MultiTenantEcommerce.Application.Users.Permissions.Queries.Permissions.GetByArea;
public record GetPermissionByAreaQuery(
    string Area) : IQuery<List<PermissionResponseDTO>>;

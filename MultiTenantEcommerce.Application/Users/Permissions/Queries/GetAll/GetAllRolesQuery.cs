using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Users.Permissions.DTOs;
using MultiTenantEcommerce.Domain.Enums;

namespace MultiTenantEcommerce.Application.Users.Permissions.Queries.GetAll;
public record GetAllRolesQuery(
    int Page = 1,
    int PageSize = 20,
    SortOptions Sort = SortOptions.TimeDesc) : IQuery<List<RoleResponseDTO>>;

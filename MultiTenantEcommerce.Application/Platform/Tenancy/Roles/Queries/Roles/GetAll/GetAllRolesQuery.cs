using MultiTenantEcommerce.Application.Platform.Tenancy.Roles.Common.DTOs;

namespace MultiTenantEcommerce.Application.Platform.Tenancy.Roles.Queries.Roles.GetAll;

public record GetAllRolesQuery(
    int Page = 1,
    int PageSize = 20,
    SortOptions Sort = SortOptions.TimeDesc) : IQuery<List<RoleResponseDTO>>;
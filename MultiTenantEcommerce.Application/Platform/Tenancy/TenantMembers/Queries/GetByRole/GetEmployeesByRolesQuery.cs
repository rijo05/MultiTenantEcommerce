using MultiTenantEcommerce.Application.Platform.Tenancy.TenantMembers.Common.DTOs;

namespace MultiTenantEcommerce.Application.Platform.Tenancy.TenantMembers.Queries.GetByRole;

public record GetTenantMembersByRolesQuery(
    Guid roleId,
    int Page = 1,
    int PageSize = 20,
    SortOptions Sort = SortOptions.TimeDesc) : IQuery<List<TenantMemberResponseDTO>>;
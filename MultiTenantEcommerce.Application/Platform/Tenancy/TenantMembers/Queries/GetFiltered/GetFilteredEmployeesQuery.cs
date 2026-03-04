using MultiTenantEcommerce.Application.Platform.Tenancy.TenantMembers.Common.DTOs;

namespace MultiTenantEcommerce.Application.Platform.Tenancy.TenantMembers.Queries.GetFiltered;

public record GetFilteredTenantMembersQuery(
    string? Name,
    string? Role,
    string? Email,
    bool? IsActive,
    int Page = 1,
    int PageSize = 20,
    SortOptions Sort = SortOptions.TimeDesc) : IQuery<List<TenantMemberResponseDTO>>;
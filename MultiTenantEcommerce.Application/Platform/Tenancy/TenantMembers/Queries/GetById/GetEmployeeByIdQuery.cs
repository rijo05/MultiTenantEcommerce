using MultiTenantEcommerce.Application.Platform.Tenancy.TenantMembers.Common.DTOs;

namespace MultiTenantEcommerce.Application.Platform.Tenancy.TenantMembers.Queries.GetById;

public record GetTenantMemberByIdQuery(
    Guid TenantMemberId) : IQuery<TenantMemberResponseDTO>;
using MultiTenantEcommerce.Application.Platform.Tenancy.TenantMembers.Common.DTOs;

namespace MultiTenantEcommerce.Application.Platform.Tenancy.TenantMembers.Queries.GetByEmail;

public record GetTenantMemberByEmailQuery(
    string Email) : IQuery<TenantMemberResponseDTO>;
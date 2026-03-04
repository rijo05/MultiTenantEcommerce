using MultiTenantEcommerce.Domain.Platform.Tenancy.Entities;
using MultiTenantEcommerce.Shared.Application.CQRS;
using MultiTenantEcommerce.Shared.Domain.ValueObjects;
using MultiTenantEcommerce.Shared.Infrastructure.Persistence;

namespace MultiTenantEcommerce.Domain.Platform.Tenancy.Interfaces.Repositories;

public interface ITenantMemberRepository : IRepository<TenantMember>
{
    public Task<TenantMember?> GetByEmail(Guid tenantId, Email email);

    public Task<List<TenantMember>> GetTenantMembersByRole(
        Guid roleId,
        int page = 1,
        int pageSize = 20,
        SortOptions sort = SortOptions.TimeDesc);

    public Task<bool> HasTenantMembersWithRole(Guid roleId);
    Task<TenantMember?> GetMemberWithPermissionsAsync(Guid tenantId, Guid userId);

    public Task<List<TenantMember>> GetFilteredAsync(
        string? name = null,
        string? role = null,
        string? email = null,
        bool? isActive = null,
        int page = 1,
        int pageSize = 20,
        SortOptions sort = SortOptions.TimeDesc);

    public Task<List<TenantMember>> GetMemberByUserId(Guid userId);

    public Task<int> CountTrialStoresForOwnerAsync(Guid ownerUserId);

    public Task<TenantMember?> GetOwnerByTenantIdAsync(Guid tenantId);

    public Task<bool> IsMemberAsync(Guid userId);

    public Task<int> CountMembers();
}
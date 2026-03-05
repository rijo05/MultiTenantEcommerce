using Microsoft.EntityFrameworkCore;
using MultiTenantEcommerce.Application.Platform.Tenancy.Interfaces;
using MultiTenantEcommerce.Domain.Platform.Identity.Entities;
using MultiTenantEcommerce.Domain.Platform.Tenancy.Entities;
using MultiTenantEcommerce.Domain.Platform.Tenancy.Entities.Auth;
using MultiTenantEcommerce.Domain.Platform.Tenancy.Enums;
using MultiTenantEcommerce.Domain.Platform.Tenancy.Interfaces.Repositories;
using MultiTenantEcommerce.Infrastructure.Persistence.Context;
using MultiTenantEcommerce.Infrastructure.Persistence.Repositories;
using MultiTenantEcommerce.Infrastructure.Platform.Tenancy.Persistence.Context;
using MultiTenantEcommerce.Shared.Application.CQRS;
using MultiTenantEcommerce.Shared.Domain.ValueObjects;

namespace MultiTenantEcommerce.Infrastructure.Platform.Tenancy.Persistence.Repositories;

public class TenantMemberRepository : Repository<TenantMember>, ITenantMemberRepository
{
    public TenantMemberRepository(TenancyDbContext appDbContext) : base(appDbContext)
    {
    }

    public async Task<List<TenantMember>> GetFilteredAsync(
        string? name = null,
        string? role = null,
        string? email = null,
        bool? isActive = null,
        int page = 1,
        int pageSize = 20,
        SortOptions sort = SortOptions.TimeDesc)
    {
        var query = _dbContext.Set<TenantMember>()
            .AsNoTracking()
            .Include(e => e.TenantMemberRoles)
            .AsSplitQuery()
            .AsQueryable();

        //if (!string.IsNullOrWhiteSpace(name))
        //    query = query.Where(p => EF.Functions.Like(p.Name, $"%{name}%"));

        //if (isActive.HasValue)
        //    query = query.Where(p => p.IsActive == isActive.Value);

        //if (!string.IsNullOrWhiteSpace(email))
        //    query = query.Where(p => p.Email != null && EF.Functions.Like(p.Email.Value, $"%{email}%"));

        if (!string.IsNullOrWhiteSpace(role))
        {
            var roleIds = _dbContext.Set<Role>()
                .Where(r => EF.Functions.Like(r.Name, $"%{role}%"))
                .Select(r => r.Id);

            query = query.Where(e => e.TenantMemberRoles.Any(er => roleIds.Contains(er.RoleId)));
        }

        return await SortAndPageAsync(query, sort, page, pageSize);
    }

    public async Task<TenantMember?> GetByEmail(Guid tenantId, Email email)
    {
        return await _dbContext.Set<TenantMember>()
            .Include(m => m.TenantMemberRoles)
            .FirstOrDefaultAsync(m => m.TenantId == tenantId &&
                                      _dbContext.User.Any(u => u.Id == m.UserId && u.Email.Value == email.Value));
    }

    public async Task<List<TenantMember>> GetTenantMembersByRole(
        Guid roleId,
        int page = 1,
        int pageSize = 20,
        SortOptions sort = SortOptions.TimeDesc)
    {
        var query = _dbContext.Set<TenantMember>()
            .Include(x => x.TenantMemberRoles)
            .AsSplitQuery()
            .Where(x => x.TenantMemberRoles.Any(er => er.RoleId == roleId));

        return await SortAndPageAsync(query, sort, page, pageSize);
    }

    public async Task<bool> HasTenantMembersWithRole(Guid roleId)
    {
        return await _dbContext.Set<TenantMemberRole>()
            .AnyAsync(x => x.RoleId == roleId);
    }

    public async Task<TenantMember?> GetMemberWithPermissionsAsync(Guid tenantId, Guid userId)
    {
        return await _dbContext.Set<TenantMember>()
            .Include(m => m.TenantMemberRoles)
            .ThenInclude(tmr => tmr.Role)
            .ThenInclude(r => r.Permissions)
            .ThenInclude(rp => rp.Permission)
            .AsSplitQuery()
            .FirstOrDefaultAsync(m => m.TenantId == tenantId && m.UserId == userId);
    }

    public override async Task<TenantMember?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Set<TenantMember>()
            .Include(x => x.TenantMemberRoles)
            .AsSplitQuery()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<TenantMember>> GetMemberByUserId(Guid userId)
    {
        return await _dbContext.Set<TenantMember>()
            .Where(tm => tm.UserId == userId)
            .ToListAsync();
    }

    public async Task<int> CountTrialStoresForOwnerAsync(Guid ownerUserId)
    {
        return await _dbContext.Set<TenantMember>()
            .Where(tm => tm.UserId == ownerUserId && tm.IsOwner == true)
            .Join(
                _dbContext.Set<Tenant>(),
                member => member.TenantId,
                tenant => tenant.Id,
                (member, tenant) => tenant)
            .CountAsync(t => t.Subscription.Status == SubscriptionStatus.Trial);
    }

    public async Task<TenantMember?> GetOwnerByTenantIdAsync(Guid tenantId)
    {
        return await _dbContext.Set<TenantMember>()
            .FirstOrDefaultAsync(tm => tm.TenantId == tenantId && tm.IsOwner == true);
    }

    public async Task<bool> IsMemberAsync(Guid userId)
    {
        return await _dbContext.Set<TenantMember>()
            .AnyAsync(x => x.UserId == userId);
    }

    public async Task<int> CountMembers()
    {   
        return await _dbContext.Set<TenantMember>().CountAsync();
    }
}
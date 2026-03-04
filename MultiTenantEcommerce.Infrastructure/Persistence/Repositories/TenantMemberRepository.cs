using Microsoft.EntityFrameworkCore;
using MultiTenantEcommerce.Domain.Platform.Tenancy.Entities;
using MultiTenantEcommerce.Domain.Platform.Tenancy.Enums;
using MultiTenantEcommerce.Domain.Platform.Tenancy.Interfaces.Repositories;
using MultiTenantEcommerce.Infrastructure.Persistence.Context;
using MultiTenantEcommerce.Shared.Application.CQRS;
using MultiTenantEcommerce.Shared.Domain.ValueObjects;
using System.Data.Entity;

namespace MultiTenantEcommerce.Infrastructure.Persistence.Repositories;

public class TenantMemberRepository : Repository<TenantMember>, ITenantMemberRepository
{
    public TenantMemberRepository(AppDbContext appDbContext) : base(appDbContext)
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
        var query = _appDbContext.TenantMembers
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
            var roleIds = _appDbContext.Roles
                .Where(r => EF.Functions.Like(r.Name, $"%{role}%"))
                .Select(r => r.Id);

            query = query.Where(e => e.TenantMemberRoles.Any(er => roleIds.Contains(er.RoleId)));
        }

        return await SortAndPageAsync(query, sort, page, pageSize);
    }

    public async Task<TenantMember?> GetByEmail(Guid tenantId, Email email)
    {
        return await _appDbContext.TenantMembers
            .Include(m => m.TenantMemberRoles)
            .FirstOrDefaultAsync(m => m.TenantId == tenantId &&
                                      _appDbContext.Users.Any(u => u.Id == m.UserId && u.Email.Value == email.Value));
    }

    public async Task<List<TenantMember>> GetTenantMembersByRole(
        Guid roleId,
        int page = 1,
        int pageSize = 20,
        SortOptions sort = SortOptions.TimeDesc)
    {
        var query = _appDbContext.TenantMembers
            .Include(x => x.TenantMemberRoles)
            .AsSplitQuery()
            .Where(x => x.TenantMemberRoles.Any(er => er.RoleId == roleId));

        return await SortAndPageAsync(query, sort, page, pageSize);
    }

    public async Task<bool> HasTenantMembersWithRole(Guid roleId)
    {
        return await _appDbContext.TenantMemberRoles
            .AnyAsync(x => x.RoleId == roleId);
    }

    public async Task<TenantMember?> GetMemberWithPermissionsAsync(Guid tenantId, Guid userId)
    {
        return await _appDbContext.TenantMembers
            .Include(m => m.TenantMemberRoles)
            .ThenInclude(tmr => tmr.Role)
            .ThenInclude(r => r.Permissions)
            .ThenInclude(rp => rp.Permission)
            .AsSplitQuery()
            .FirstOrDefaultAsync(m => m.TenantId == tenantId && m.UserId == userId);
    }

    public override async Task<TenantMember?> GetByIdAsync(Guid id)
    {
        return await _appDbContext.TenantMembers
            .Include(x => x.TenantMemberRoles)
            .AsSplitQuery()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<TenantMember>> GetMemberByUserId(Guid userId)
    {
        return await _appDbContext.TenantMembers
            .Where(tm => tm.UserId == userId)
            .ToListAsync();
    }

    public async Task<int> CountTrialStoresForOwnerAsync(Guid ownerUserId)
    {
        return await _appDbContext.TenantMembers
            .Where(tm => tm.UserId == ownerUserId && tm.IsOwner == true)
            .Join(
                _appDbContext.Tenants,
                member => member.TenantId,
                tenant => tenant.Id,
                (member, tenant) => tenant)
            .CountAsync(t => t.Subscription.Status == SubscriptionStatus.Trial);
    }

    public async Task<TenantMember?> GetOwnerByTenantIdAsync(Guid tenantId)
    {
        return await _appDbContext.TenantMembers
            .FirstOrDefaultAsync(tm => tm.TenantId == tenantId && tm.IsOwner == true);
    }

    public async Task<bool> IsMemberAsync(Guid userId)
    {
        return await _appDbContext.TenantMembers
            .AnyAsync(x => x.UserId == userId);
    }

    public async Task<int> CountMembers()
    {   
        return await _appDbContext.TenantMembers.CountAsync();
    }
}
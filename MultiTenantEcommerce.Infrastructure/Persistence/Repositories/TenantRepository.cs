using Microsoft.EntityFrameworkCore;
using MultiTenantEcommerce.Domain.Platform.Tenancy.Entities;
using MultiTenantEcommerce.Domain.Platform.Tenancy.Interfaces.Repositories;
using MultiTenantEcommerce.Infrastructure.Persistence.Context;
using MultiTenantEcommerce.Shared.Application.CQRS;

namespace MultiTenantEcommerce.Infrastructure.Persistence.Repositories;

public class TenantRepository : Repository<Tenant>, ITenantRepository
{
    public TenantRepository(AppDbContext appDbContext) : base(appDbContext)
    {
    }

    public async Task<Tenant?> GetByCompanyNameAllIncluded(string name)
    {
        return await _appDbContext.Tenants
            .Include(x => x.Subscription)
            .AsSplitQuery()
            .FirstOrDefaultAsync(x => x.Name == name);
    }

    public async Task<List<Tenant>> GetFilteredAsync(
        string? companyName,
        int page = 1,
        int pageSize = 20,
        SortOptions sort = SortOptions.TimeDesc)
    {
        var query = _appDbContext.Tenants
            .AsNoTracking()
            .Include(x => x.Subscription)
            .AsQueryable()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(companyName))
            query = query.Where(p => EF.Functions.Like(p.Name, $"%{companyName}%"));


        return await SortAndPageAsync(query, sort, page, pageSize);
    }

    public override async Task<Tenant?> GetByIdAsync(Guid tenantId)
    {
        return await _appDbContext.Tenants
            .Include(x => x.Subscription)
            .AsSplitQuery()
            .FirstOrDefaultAsync(x => x.Id == tenantId);
    }

    public async Task<List<Tenant>> GetTenantsForUserAsync(Guid userId)
    {
        return await (
                from tenant in _appDbContext.Tenants
                join member in _appDbContext.TenantMembers on tenant.Id equals member.TenantId
                where member.UserId == userId
                select tenant).ToListAsync();
    }

    public async Task<bool> IsSubdomainAvailableAsync(string subdomain)
    {
        return await _appDbContext.Tenants
            .AnyAsync(x => x.SubDomain == subdomain);
    }
}
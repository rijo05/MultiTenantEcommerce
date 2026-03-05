using Microsoft.EntityFrameworkCore;
using MultiTenantEcommerce.Domain.Platform.Tenancy.Entities;
using MultiTenantEcommerce.Domain.Platform.Tenancy.Interfaces.Repositories;
using MultiTenantEcommerce.Infrastructure.Persistence.Context;
using MultiTenantEcommerce.Infrastructure.Persistence.Repositories;
using MultiTenantEcommerce.Infrastructure.Platform.Tenancy.Persistence.Context;
using MultiTenantEcommerce.Shared.Application.CQRS;

namespace MultiTenantEcommerce.Infrastructure.Platform.Tenancy.Persistence.Repositories;

public class TenantRepository : Repository<Tenant>, ITenantRepository
{
    public TenantRepository(TenancyDbContext appDbContext) : base(appDbContext)
    {
    }

    public async Task<Tenant?> GetByCompanyNameAllIncluded(string name)
    {
        return await _dbContext.Set<Tenant>()
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
        var query = _dbContext.Set<Tenant>()
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
        return await _dbContext.Set<Tenant>()
            .Include(x => x.Subscription)
            .AsSplitQuery()
            .FirstOrDefaultAsync(x => x.Id == tenantId);
    }

    public async Task<List<Tenant>> GetTenantsForUserAsync(Guid userId)
    {
        return await (
                from tenant in _dbContext.Set<Tenant>()
                join member in _dbContext.Set<TenantMember>() on tenant.Id equals member.TenantId
                where member.UserId == userId
                select tenant).ToListAsync();
    }

    public async Task<bool> IsSubdomainAvailableAsync(string subdomain)
    {
        return await _dbContext.Set<Tenant>()
            .AnyAsync(x => x.SubDomain == subdomain);
    }
}
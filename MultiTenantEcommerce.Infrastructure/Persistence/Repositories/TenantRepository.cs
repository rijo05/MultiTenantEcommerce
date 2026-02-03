using Microsoft.EntityFrameworkCore;
using MultiTenantEcommerce.Domain.Enums;
using MultiTenantEcommerce.Domain.Tenants.Entities;
using MultiTenantEcommerce.Domain.Tenants.Interfaces;
using MultiTenantEcommerce.Infrastructure.Persistence.Context;

namespace MultiTenantEcommerce.Infrastructure.Persistence.Repositories;
public class TenantRepository : Repository<Tenant>, ITenantRepository
{
    public TenantRepository(AppDbContext appDbContext) : base(appDbContext) { }

    public async Task<Tenant?> GetByCompanyNameAllIncluded(string name)
    {
        return await _appDbContext.Tenants
            .Include(x => x.Subscription)
                .ThenInclude(x => x.Plan)
            .Include(x => x.ShippingProviders)
            .AsSplitQuery()
            .FirstOrDefaultAsync(x => x.Name == name);
    }

    public override async Task<Tenant?> GetByIdAsync(Guid tenantId)
    {
        return await _appDbContext.Tenants
            .Include(x => x.Subscription)
                .ThenInclude(x => x.Plan)
            .Include(x => x.ShippingProviders)
            .AsSplitQuery()
            .FirstOrDefaultAsync(x => x.Id == tenantId);
    }

    public async Task<Tenant?> GetByStripeCustomerId(string stripeCustomerId)
    {
        return await _appDbContext.Tenants
            .Include(x => x.Subscription)
                .ThenInclude(x => x.Plan)
            .AsSplitQuery()
            .FirstOrDefaultAsync(x => x.StripeCustomerId == stripeCustomerId);
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
                .ThenInclude(x => x.Plan)
            .Include(x => x.ShippingProviders)
            .AsQueryable()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(companyName))
            query = query.Where(p => EF.Functions.Like(p.Name, $"%{companyName}%"));


        return await SortAndPageAsync(query, sort, page, pageSize);
    }
}

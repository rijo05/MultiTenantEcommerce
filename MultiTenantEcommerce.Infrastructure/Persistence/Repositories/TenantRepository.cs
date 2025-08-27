using Microsoft.EntityFrameworkCore;
using MultiTenantEcommerce.Domain.Enums;
using MultiTenantEcommerce.Domain.Tenancy.Entities;
using MultiTenantEcommerce.Domain.Tenancy.Interfaces;
using MultiTenantEcommerce.Infrastructure.Persistence.Context;

namespace MultiTenantEcommerce.Infrastructure.Persistence.Repositories;
public class TenantRepository : Repository<Tenant>, ITenantRepository
{
    public TenantRepository(AppDbContext appDbContext, TenantContext tenantContext) : base(appDbContext, tenantContext) { }

    public async Task<Tenant?> GetByCompanyName(string name)
    {
        return await _appDbContext.Tenants.FirstOrDefaultAsync(x => x.Name == name);
    }

    public async Task<List<Tenant>> GetFilteredAsync(
        int page = 1,
        int pageSize = 20,
        SortOptions? sort = null)
    {
        var query = _appDbContext.Tenants.AsQueryable();

        return await SortAndPageAsync(query, sort, page, pageSize);
    }
}

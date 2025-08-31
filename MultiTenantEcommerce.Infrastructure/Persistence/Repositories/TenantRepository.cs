using Microsoft.EntityFrameworkCore;
using MultiTenantEcommerce.Domain.Enums;
using MultiTenantEcommerce.Domain.Tenants.Entities;
using MultiTenantEcommerce.Domain.Tenants.Interfaces;
using MultiTenantEcommerce.Infrastructure.Persistence.Context;
using System.Xml.Linq;

namespace MultiTenantEcommerce.Infrastructure.Persistence.Repositories;
public class TenantRepository : Repository<Tenant>, ITenantRepository
{
    public TenantRepository(AppDbContext appDbContext, TenantContext tenantContext) : base(appDbContext, tenantContext) { }

    public async Task<Tenant?> GetByCompanyName(string name)
    {
        return await _appDbContext.Tenants.FirstOrDefaultAsync(x => x.Name == name);
    }

    public async Task<List<Tenant>> GetFilteredAsync(
        string? companyName,
        int page = 1,
        int pageSize = 20,
        SortOptions sort = SortOptions.TimeDesc)
    {
        var query = _appDbContext.Tenants.AsQueryable();

        if (!string.IsNullOrWhiteSpace(companyName))
            query = query.Where(p => EF.Functions.Like(p.Name, $"%{companyName}%"));


        return await SortAndPageAsync(query, sort, page, pageSize);
    }
}

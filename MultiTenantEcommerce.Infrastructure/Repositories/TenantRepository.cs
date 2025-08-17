using Microsoft.EntityFrameworkCore;
using MultiTenantEcommerce.Domain.Entities;
using MultiTenantEcommerce.Domain.Interfaces;
using MultiTenantEcommerce.Infrastructure.Context;

namespace MultiTenantEcommerce.Infrastructure.Repositories;
public class TenantRepository : Repository<Tenant>, ITenantRepository
{
    public TenantRepository(AppDbContext appDbContext) : base(appDbContext) { }

    public async Task<Tenant?> GetByCompanyName(string companyName)
    {
        return await _appDbContext.Tenants.FirstOrDefaultAsync(x => x.CompanyName == companyName);
    }
}

using MultiTenantEcommerce.Domain.Entities;
using MultiTenantEcommerce.Domain.Interfaces;

namespace MultiTenantEcommerce.Infrastructure.Repositories;
public class TenantRepository : Repository<Tenant>, ITenantRepository
{
    public TenantRepository(AppDbContext appDbContext) : base(appDbContext) { }
}

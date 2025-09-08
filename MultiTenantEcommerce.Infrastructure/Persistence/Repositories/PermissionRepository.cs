using MultiTenantEcommerce.Domain.Users.Entities.Permissions;
using MultiTenantEcommerce.Domain.Users.Interfaces.Permissions;
using MultiTenantEcommerce.Infrastructure.Persistence.Context;
using System.Data.Entity;

namespace MultiTenantEcommerce.Infrastructure.Persistence.Repositories;
public class PermissionRepository : Repository<Permission>, IPermissionRepository
{
    public PermissionRepository(AppDbContext appDbContext, TenantContext tenantContext) : base(appDbContext, tenantContext)
    {
    }

    public async Task<Permission?> GetByNameAsync(string name)
    {
        return await _appDbContext.Permissions.FirstOrDefaultAsync(x => x.Name == name);
    }

    public async Task<List<Permission>> GetByAction(string action)
    {
        return await _appDbContext.Permissions.Where(x => x.Action == action).ToListAsync();
    }

    public async Task<List<Permission>> GetByArea(string area)
    {
        return await _appDbContext.Permissions.Where(x => x.Area == area).ToListAsync();
    }
}

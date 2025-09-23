using Microsoft.EntityFrameworkCore;
using MultiTenantEcommerce.Domain.Users.Entities.Permissions;
using MultiTenantEcommerce.Domain.Users.Interfaces.Permissions;
using MultiTenantEcommerce.Infrastructure.Persistence.Context;

namespace MultiTenantEcommerce.Infrastructure.Persistence.Repositories;
public class PermissionRepository : Repository<Permission>, IPermissionRepository
{
    public PermissionRepository(AppDbContext appDbContext, TenantContext tenantContext) : base(appDbContext, tenantContext)
    {
    }

    public async Task<List<Permission>> GetByAction(string action)
    {
        return await _appDbContext.Permissions
            .Where(x => x.Action.ToLower() == action.ToLower()).ToListAsync();
    }

    public async Task<List<Permission>> GetByArea(string area)
    {
        return await _appDbContext.Permissions
            .Where(x => x.Area.ToLower() == area.ToLower()).ToListAsync();
    }
}

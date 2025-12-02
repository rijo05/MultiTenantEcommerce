using Microsoft.EntityFrameworkCore;
using MultiTenantEcommerce.Domain.Users.Entities.Permissions;
using MultiTenantEcommerce.Domain.Users.Interfaces.Permissions;
using MultiTenantEcommerce.Infrastructure.Persistence.Context;

namespace MultiTenantEcommerce.Infrastructure.Persistence.Repositories;
public class PermissionRepository : Repository<Permission>, IPermissionRepository
{
    public PermissionRepository(AppDbContext appDbContext) : base(appDbContext)
    {
    }

    public async Task<List<Permission>> GetByAction(string action)
    {
        return await _appDbContext.Permissions
            .AsNoTracking()
            .AsSplitQuery()
            .Where(x => x.Action.ToLower() == action.ToLower()).ToListAsync();
    }

    public async Task<List<Permission>> GetByArea(string area)
    {
        return await _appDbContext.Permissions
            .AsNoTracking()
            .AsSplitQuery()
            .Where(x => x.Area.ToLower() == area.ToLower()).ToListAsync();
    }

    public async Task<List<string>> GetPermissionNamesByIdsAsync(IEnumerable<Guid> ids)
    {
        var distinctIds = ids.Distinct().ToList();
        return await _appDbContext.Permissions
            .AsNoTracking()
            .AsSplitQuery()
            .Where(p => distinctIds.Contains(p.Id))
            .Select(p => p.Name)
            .ToListAsync();
    }
}

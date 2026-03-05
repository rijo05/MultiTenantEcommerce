using Microsoft.EntityFrameworkCore;
using MultiTenantEcommerce.Domain.Platform.Tenancy.Entities;
using MultiTenantEcommerce.Domain.Platform.Tenancy.Entities.Auth;
using MultiTenantEcommerce.Domain.Platform.Tenancy.Interfaces.Repositories;
using MultiTenantEcommerce.Infrastructure.Persistence.Context;
using MultiTenantEcommerce.Infrastructure.Persistence.Repositories;
using MultiTenantEcommerce.Infrastructure.Platform.Tenancy.Persistence.Context;

namespace MultiTenantEcommerce.Infrastructure.Platform.Tenancy.Persistence.Repositories;

public class PermissionRepository : Repository<Permission>, IPermissionRepository
{
    public PermissionRepository(TenancyDbContext appDbContext) : base(appDbContext)
    {
    }

    public async Task<List<Permission>> GetByAction(string action)
    {
        return await _dbContext.Set<Permission>()
            .AsNoTracking()
            .AsSplitQuery()
            .Where(x => x.Action.ToLower() == action.ToLower()).ToListAsync();
    }

    public async Task<List<Permission>> GetByArea(string area)
    {
        return await _dbContext.Set<Permission>()
            .AsNoTracking()
            .AsSplitQuery()
            .Where(x => x.Area.ToLower() == area.ToLower()).ToListAsync();
    }

    public async Task<List<string>> GetPermissionNamesByIdsAsync(IEnumerable<Guid> ids)
    {
        var distinctIds = ids.Distinct().ToList();
        return await _dbContext.Set<Permission>()
            .AsNoTracking()
            .AsSplitQuery()
            .Where(p => distinctIds.Contains(p.Id))
            .Select(p => p.Name)
            .ToListAsync();
    }
}
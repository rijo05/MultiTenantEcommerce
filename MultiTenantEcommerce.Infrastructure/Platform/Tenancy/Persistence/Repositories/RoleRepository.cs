using Microsoft.EntityFrameworkCore;
using MultiTenantEcommerce.Domain.Platform.Tenancy.Entities.Auth;
using MultiTenantEcommerce.Domain.Platform.Tenancy.Interfaces.Repositories;
using MultiTenantEcommerce.Infrastructure.Persistence.Context;
using MultiTenantEcommerce.Infrastructure.Persistence.Repositories;
using MultiTenantEcommerce.Infrastructure.Platform.Tenancy.Persistence.Context;
using MultiTenantEcommerce.Shared.Application.CQRS;

namespace MultiTenantEcommerce.Infrastructure.Platform.Tenancy.Persistence.Repositories;

public class RoleRepository : Repository<Role>, IRoleRepository
{
    public RoleRepository(TenancyDbContext appDbContext) : base(appDbContext)
    {
    }

    public async Task<Role?> GetByNameAsync(string name)
    {
        return await _dbContext.Set<Role>()
            .Include(x => x.Permissions)
            .AsSplitQuery()
            .FirstOrDefaultAsync(x => x.Name == name);
    }

    public async Task<Role?> GetByIdWithPermisionsAsync(Guid id)
    {
        return await _dbContext.Set<Role>()
            .Include(x => x.Permissions)
            .ThenInclude(rp => rp.Permission)
            .AsSplitQuery()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<Role>> GetRolesWithPermissionsByIdsAsync(List<Guid> roleIds)
    {
        return await _dbContext.Set<Role>()
            .Include(r => r.Permissions)
            .Where(r => roleIds.Contains(r.Id))
            .AsSplitQuery()
            .ToListAsync();
    }


    public async Task<List<Role>> GetAllRolesAndPermissions(int page, int pageSize, SortOptions sortOptions)
    {
        var rolesquery = _dbContext.Roles
            .Include(x => x.Permissions)
            .AsSplitQuery();

        return await SortAndPageAsync(rolesquery, sortOptions, page, pageSize);
    }
}
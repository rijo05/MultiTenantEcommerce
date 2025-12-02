using Microsoft.EntityFrameworkCore;
using MultiTenantEcommerce.Domain.Enums;
using MultiTenantEcommerce.Domain.Users.Entities.Permissions;
using MultiTenantEcommerce.Domain.Users.Interfaces.Permissions;
using MultiTenantEcommerce.Infrastructure.Persistence.Context;

namespace MultiTenantEcommerce.Infrastructure.Persistence.Repositories;
public class RoleRepository : Repository<Role>, IRoleRepository
{
    public RoleRepository(AppDbContext appDbContext) : base(appDbContext)
    {
    }

    public async Task<Role?> GetByNameAsync(string name)
    {
        return await _appDbContext.Roles
            .Include(x => x.Permissions)
            .AsSplitQuery()
            .FirstOrDefaultAsync(x => x.Name == name);
    }

    public async Task<Role?> GetByIdWithPermisionsAsync(Guid id)
    {
        return await _appDbContext.Roles
            .Include(x => x.Permissions)
            .AsSplitQuery()
            .FirstAsync(x => x.Id == id);
    }

    public async Task<List<Role>> GetRolesWithPermissionsByIdsAsync(List<Guid> roleIds)
    {
        return await _appDbContext.Roles
            .Include(r => r.Permissions)
            .Where(r => roleIds.Contains(r.Id))
            .AsSplitQuery()
            .ToListAsync();
    }


    public async Task<List<Role>> GetAllRolesAndPermissions(int page, int pageSize, SortOptions sortOptions)
    {
        var rolesquery = _appDbContext.Roles
            .Include(x => x.Permissions)
            .AsSplitQuery();

        return await SortAndPageAsync(rolesquery, sortOptions, page, pageSize);
    }
}

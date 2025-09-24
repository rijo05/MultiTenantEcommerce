using Microsoft.EntityFrameworkCore;
using MultiTenantEcommerce.Domain.Enums;
using MultiTenantEcommerce.Domain.Users.Entities.Permissions;
using MultiTenantEcommerce.Domain.Users.Interfaces.Permissions;
using MultiTenantEcommerce.Infrastructure.Persistence.Context;

namespace MultiTenantEcommerce.Infrastructure.Persistence.Repositories;
public class RoleRepository : Repository<Role>, IRoleRepository
{
    public RoleRepository(AppDbContext appDbContext, TenantContext tenantContext) : base(appDbContext, tenantContext)
    {
    }

    public async Task<Role?> GetByNameAsync(string name)
    {
        return await _appDbContext.Roles.Include(x => x.Permissions)
            .FirstOrDefaultAsync(x => x.Name == name);
    }

    public async Task<Role?> GetRoleAndPermissionById(Guid id)
    {
        return await _appDbContext.Roles.Include(x => x.Permissions)
            .FirstAsync(x => x.Id == id);
    }


    public async Task<List<Role>> GetAllRolesAndPermissions(int page, int pageSize, SortOptions sortOptions)
    {
        var rolesquery = _appDbContext.Roles.Include(x => x.Permissions);

        return await SortAndPageAsync(rolesquery, sortOptions, page, pageSize);
    }
}

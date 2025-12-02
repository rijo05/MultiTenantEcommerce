using MultiTenantEcommerce.Domain.Common.Interfaces;
using MultiTenantEcommerce.Domain.Enums;
using MultiTenantEcommerce.Domain.Users.Entities.Permissions;

namespace MultiTenantEcommerce.Domain.Users.Interfaces.Permissions;
public interface IRoleRepository : IRepository<Role>
{
    public Task<Role?> GetByNameAsync(string name);

    public Task<Role?> GetByIdWithPermisionsAsync(Guid id);

    public Task<List<Role>> GetRolesWithPermissionsByIdsAsync(List<Guid> roleIds);

    public Task<List<Role>> GetAllRolesAndPermissions(int page, int pageSize, SortOptions sortOptions);
}

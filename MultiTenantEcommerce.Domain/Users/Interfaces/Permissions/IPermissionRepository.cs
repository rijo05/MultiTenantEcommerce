using MultiTenantEcommerce.Domain.Common.Interfaces;
using MultiTenantEcommerce.Domain.Users.Entities.Permissions;

namespace MultiTenantEcommerce.Domain.Users.Interfaces.Permissions;
public interface IPermissionRepository : IRepository<Permission>
{
    public Task<List<Permission>> GetByArea(
        string area);
    public Task<List<Permission>> GetByAction(
        string action);
}

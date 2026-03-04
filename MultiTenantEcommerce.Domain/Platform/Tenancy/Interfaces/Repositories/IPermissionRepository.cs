using MultiTenantEcommerce.Domain.Platform.Tenancy.Entities.Auth;
using MultiTenantEcommerce.Shared.Infrastructure.Persistence;

namespace MultiTenantEcommerce.Domain.Platform.Tenancy.Interfaces.Repositories;

public interface IPermissionRepository : IRepository<Permission>
{
    public Task<List<Permission>> GetByArea(string area);

    public Task<List<Permission>> GetByAction(string action);

    public Task<List<string>> GetPermissionNamesByIdsAsync(IEnumerable<Guid> ids);
}
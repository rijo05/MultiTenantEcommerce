using MultiTenantEcommerce.Domain.Platform.Tenancy.Entities.Auth;
using MultiTenantEcommerce.Shared.Application.CQRS;
using MultiTenantEcommerce.Shared.Infrastructure.Persistence;

namespace MultiTenantEcommerce.Domain.Platform.Tenancy.Interfaces.Repositories;

public interface IRoleRepository : IRepository<Role>
{
    public Task<Role?> GetByNameAsync(string name);

    public Task<Role?> GetByIdWithPermisionsAsync(Guid id);

    public Task<List<Role>> GetRolesWithPermissionsByIdsAsync(List<Guid> roleIds);

    public Task<List<Role>> GetAllRolesAndPermissions(int page, int pageSize, SortOptions sortOptions);
}
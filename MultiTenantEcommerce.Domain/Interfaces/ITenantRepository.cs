using MultiTenantEcommerce.Domain.Entities;
using MultiTenantEcommerce.Domain.Enums;

namespace MultiTenantEcommerce.Domain.Interfaces;
public interface ITenantRepository : IRepository<Tenant>
{
    public Task<Tenant?> GetByCompanyName(string companyName);

    public Task<List<Tenant>> GetFilteredAsync(
        int page = 1,
        int pageSize = 20,
        SortOptions? sort = null);
}

using MultiTenantEcommerce.Domain.Common.Interfaces;
using MultiTenantEcommerce.Domain.Enums;
using MultiTenantEcommerce.Domain.Tenancy.Entities;

namespace MultiTenantEcommerce.Domain.Tenancy.Interfaces;
public interface ITenantRepository : IRepository<Tenant>
{
    public Task<Tenant?> GetByCompanyName(string companyName);

    public Task<List<Tenant>> GetFilteredAsync(
        int page = 1,
        int pageSize = 20,
        SortOptions? sort = null);
}

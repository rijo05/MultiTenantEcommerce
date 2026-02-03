using MultiTenantEcommerce.Domain.Common.Interfaces;
using MultiTenantEcommerce.Domain.Enums;
using MultiTenantEcommerce.Domain.Tenants.Entities;

namespace MultiTenantEcommerce.Domain.Tenants.Interfaces;
public interface ITenantRepository : IRepository<Tenant>
{
    public Task<Tenant?> GetByCompanyNameAllIncluded(string companyName);

    public Task<List<Tenant>> GetFilteredAsync(
        string? companyName,
        int page = 1,
        int pageSize = 20,
        SortOptions sort = SortOptions.TimeDesc);

    public Task<Tenant?> GetByStripeCustomerId (string stripeCustomerId);
}

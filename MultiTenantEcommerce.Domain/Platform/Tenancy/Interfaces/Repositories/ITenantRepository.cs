using MultiTenantEcommerce.Domain.Platform.Tenancy.Entities;
using MultiTenantEcommerce.Shared.Application.CQRS;
using MultiTenantEcommerce.Shared.Infrastructure.Persistence;
using System.Runtime.CompilerServices;

namespace MultiTenantEcommerce.Domain.Platform.Tenancy.Interfaces.Repositories;

public interface ITenantRepository : IRepository<Tenant>
{
    public Task<Tenant?> GetByCompanyNameAllIncluded(string companyName);

    public Task<List<Tenant>> GetFilteredAsync(
        string? companyName,
        int page = 1,
        int pageSize = 20,
        SortOptions sort = SortOptions.TimeDesc);

    public Task<List<Tenant>> GetTenantsForUserAsync(Guid userId);

    public Task<bool> IsSubdomainAvailableAsync(string subdomain);
}
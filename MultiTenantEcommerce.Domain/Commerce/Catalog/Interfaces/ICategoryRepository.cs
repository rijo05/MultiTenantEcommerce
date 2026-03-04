using MultiTenantEcommerce.Domain.Commerce.Catalog.Entities;
using MultiTenantEcommerce.Shared.Application;
using MultiTenantEcommerce.Shared.Application.CQRS;
using MultiTenantEcommerce.Shared.Infrastructure.Persistence;

namespace MultiTenantEcommerce.Domain.Commerce.Catalog.Interfaces;

public interface ICategoryRepository : IRepository<Category>
{
    public Task<Category?> GetByExactNameAsync(string name);

    public Task<PaginatedList<Category>> GetFilteredAsync(
        string? name = null,
        string? description = null,
        bool? isActive = null,
        int page = 1,
        int pageSize = 20,
        SortOptions sort = SortOptions.TimeDesc);
}
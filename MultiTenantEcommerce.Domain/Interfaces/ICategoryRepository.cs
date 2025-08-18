using MultiTenantEcommerce.Domain.Entities;
using MultiTenantEcommerce.Domain.Enums;

namespace MultiTenantEcommerce.Domain.Interfaces;

public interface ICategoryRepository : IRepository<Category>
{
    public Task<Category?> GetByExactNameAsync(string name);

    public Task<List<Category>> GetFilteredAsync(
    string? name = null,
    string? description = null,
    bool? isActive = null,
    int page = 1,
    int pageSize = 20,
    SortOptions? sort = null);
}

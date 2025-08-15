using MultiTenantEcommerce.Domain.Entities;

namespace MultiTenantEcommerce.Domain.Interfaces;

public interface ICategoryRepository : IRepository<Category>
{
    public Task<List<Category>> SearchByNameAsync(string name);

    public Task<Category?> GetByExactNameAsync(string name);
}

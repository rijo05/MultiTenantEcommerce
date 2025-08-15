using MultiTenantEcommerce.Domain.Entities;

namespace MultiTenantEcommerce.Domain.Interfaces;

public interface IProductRepository : IRepository<Product>
{
    public Task<List<Product>> GetByCategoryIdAsync(Guid categoryId);
    public Task<List<Product>> GetFilteredAsync(
        Guid? categoryId = null,
        string? name = null,
        decimal? minPrice = null,
        decimal? maxPrice = null,
        bool? isActive = null,
        int page = 1,
        int pageSize = 20,
        string? sort = null);
}

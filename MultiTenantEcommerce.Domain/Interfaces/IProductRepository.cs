using MultiTenantEcommerce.Domain.Entities;
using MultiTenantEcommerce.Domain.Enums;

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
        SortOptions? sort = null);

    public Task AddBulkAsync(List<Product> products);
}

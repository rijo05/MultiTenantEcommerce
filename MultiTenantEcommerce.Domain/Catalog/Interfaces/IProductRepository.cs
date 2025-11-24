using MultiTenantEcommerce.Domain.Catalog.Entities;
using MultiTenantEcommerce.Domain.Common.Interfaces;
using MultiTenantEcommerce.Domain.Enums;

namespace MultiTenantEcommerce.Domain.Catalog.Interfaces;

public interface IProductRepository : IRepository<Product>
{
    public Task<bool> HasProductsInCategoryAsync(Guid categoryId);
    public Task<IEnumerable<Product>> GetFilteredAsync(
        Guid? categoryId = null,
        string? name = null,
        decimal? minPrice = null,
        decimal? maxPrice = null,
        bool? isActive = null,
        int page = 1,
        int pageSize = 20,
        SortOptions sort = SortOptions.TimeDesc);

    public Task AddBulkAsync(IEnumerable<Product> products);
}

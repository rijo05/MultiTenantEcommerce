using MultiTenantEcommerce.Domain.Commerce.Catalog.Entities;
using MultiTenantEcommerce.Domain.Commerce.Catalog.Enums;
using MultiTenantEcommerce.Shared.Application;
using MultiTenantEcommerce.Shared.Application.CQRS;
using MultiTenantEcommerce.Shared.Infrastructure.Persistence;

namespace MultiTenantEcommerce.Domain.Commerce.Catalog.Interfaces;

public interface IProductRepository : IRepository<Product>
{
    public Task<bool> HasProductsInCategoryAsync(Guid categoryId);

    public Task<PaginatedList<Product>> GetFilteredAsync(
        Guid? categoryId = null,
        string? name = null,
        decimal? minPrice = null,
        decimal? maxPrice = null,
        bool? isActive = null,
        List<StockStatus>? stockStatuses = null,
        int page = 1,
        int pageSize = 20,
        SortOptions sort = SortOptions.TimeDesc);

    public Task<Dictionary<Guid, List<string>>> GetProductImageKeysAsync(List<Guid> productIds)

    public Task AddBulkAsync(IEnumerable<Product> products);
}
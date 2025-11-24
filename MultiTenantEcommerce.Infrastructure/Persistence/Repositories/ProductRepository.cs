using Microsoft.EntityFrameworkCore;
using MultiTenantEcommerce.Domain.Catalog.Entities;
using MultiTenantEcommerce.Domain.Catalog.Interfaces;
using MultiTenantEcommerce.Domain.Enums;
using MultiTenantEcommerce.Infrastructure.Persistence.Context;

namespace MultiTenantEcommerce.Infrastructure.Persistence.Repositories;

public class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(AppDbContext appDbContext, TenantContext tenantContext)
    : base(appDbContext, tenantContext) { }

    public async Task<bool> HasProductsInCategoryAsync(Guid categoryId)
    {
        return await _appDbContext.Products.AnyAsync(x => x.CategoryId == categoryId);
    }

    public async Task<IEnumerable<Product>> GetFilteredAsync(
    Guid? categoryId = null,
    string? name = null,
    decimal? minPrice = null,
    decimal? maxPrice = null,
    bool? isActive = null,
    int page = 1,
    int pageSize = 20,
    SortOptions sort = SortOptions.TimeDesc)
    {
        var query = _appDbContext.Products
            .Include(p => p.Category)
            .Include(p => p.Images)
            .AsQueryable();

        if (categoryId.HasValue)
            query = query.Where(p => p.CategoryId == categoryId.Value);

        if (!string.IsNullOrWhiteSpace(name))
            query = query.Where(p => EF.Functions.Like(p.Name, $"%{name}%"));

        if (minPrice.HasValue)
            query = query.Where(p => p.Price.Value >= minPrice);

        if (maxPrice.HasValue)
            query = query.Where(p => p.Price.Value <= maxPrice);

        if (isActive.HasValue)
            query = query.Where(p => p.IsActive == isActive.Value);


        return await SortAndPageAsync(query, sort, page, pageSize);
    }

    public async Task AddBulkAsync(IEnumerable<Product> products)
    {
        await _appDbContext.Products.AddRangeAsync(products);
    }
}

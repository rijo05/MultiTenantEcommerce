using Microsoft.EntityFrameworkCore;
using MultiTenantEcommerce.Domain.Entities;
using MultiTenantEcommerce.Domain.Enums;
using MultiTenantEcommerce.Domain.Interfaces;
using MultiTenantEcommerce.Infrastructure.Context;

namespace MultiTenantEcommerce.Infrastructure.Repositories;

public class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(AppDbContext appDbContext, TenantContext tenantContext)
    : base(appDbContext, tenantContext) { }

    public async Task<List<Product>> GetByCategoryIdAsync(Guid categoryId)
    {
        return await _appDbContext.Products
                        .Where(p => p.CategoryId == categoryId)
                        .ToListAsync();
    }

    public async Task<List<Product>> GetFilteredAsync(
    Guid? categoryId = null,
    string? name = null,
    decimal? minPrice = null,
    decimal? maxPrice = null,
    bool? isActive = null,
    int page = 1,
    int pageSize = 20,
    SortOptions? sort = null)
    {
        var query = _appDbContext.Products.AsQueryable();

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

    public async Task AddBulkAsync(List<Product> products)
    {
        await _appDbContext.Products.AddRangeAsync(products);
    }

    public async Task<Product?> GetBySKUAsync(string sku)
    {
        return await _appDbContext.Products.FirstOrDefaultAsync(x => x.SKU == sku);
    }
}

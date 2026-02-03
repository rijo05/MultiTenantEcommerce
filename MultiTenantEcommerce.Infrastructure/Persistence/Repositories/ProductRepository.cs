using Microsoft.EntityFrameworkCore;
using MultiTenantEcommerce.Domain.Catalog.Entities;
using MultiTenantEcommerce.Domain.Catalog.Interfaces;
using MultiTenantEcommerce.Domain.Enums;
using MultiTenantEcommerce.Infrastructure.Persistence.Context;

namespace MultiTenantEcommerce.Infrastructure.Persistence.Repositories;

public class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(AppDbContext appDbContext)
    : base(appDbContext) { }

    public async Task<bool> HasProductsInCategoryAsync(Guid categoryId)
    {
        return await _appDbContext.Products.AnyAsync(x => x.CategoryId == categoryId);
    }

    public override async Task<Product?> GetByIdAsync(Guid productId)
    {
        return await _appDbContext.Products
            .Include(x => x.Images)
            .AsSplitQuery()
            .FirstOrDefaultAsync(x => x.Id == productId);
    }

    public override async Task<List<Product>> GetByIdsAsync(IEnumerable<Guid> ids)
    {
        return await _appDbContext.Products
            .Include(x => x.Images)
            .Where(x => ids.Contains(x.Id))
            .AsSplitQuery()
            .ToListAsync();
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
            .AsNoTracking()
            .Include(p => p.Images)
            .AsSplitQuery()
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

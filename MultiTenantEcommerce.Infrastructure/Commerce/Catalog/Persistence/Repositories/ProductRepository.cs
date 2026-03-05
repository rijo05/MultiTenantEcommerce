using Microsoft.EntityFrameworkCore;
using MultiTenantEcommerce.Domain.Commerce.Catalog.Entities;
using MultiTenantEcommerce.Domain.Commerce.Catalog.Enums;
using MultiTenantEcommerce.Domain.Commerce.Catalog.Interfaces;
using MultiTenantEcommerce.Infrastructure.Persistence.Catalog.Context;
using MultiTenantEcommerce.Infrastructure.Persistence.Context;
using MultiTenantEcommerce.Infrastructure.Persistence.Repositories;
using MultiTenantEcommerce.Shared.Application;
using MultiTenantEcommerce.Shared.Application.CQRS;

namespace MultiTenantEcommerce.Infrastructure.Persistence.Catalog.Repositories;

public class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(CatalogDbContext appDbContext)
        : base(appDbContext)
    {
    }

    public async Task<bool> HasProductsInCategoryAsync(Guid categoryId)
    {
        return await _dbContext.Set<Product>().AnyAsync(x => x.CategoryId == categoryId);
    }

    public async Task<PaginatedList<Product>> GetFilteredAsync(
        Guid? categoryId = null,
        string? name = null,
        decimal? minPrice = null,
        decimal? maxPrice = null,
        bool? isActive = null,
        List<StockStatus>? stockStatuses = null,
        int page = 1,
        int pageSize = 20,
        SortOptions sort = SortOptions.TimeDesc)
    {
        var query = _dbContext.Set<Product>()
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

        if (stockStatuses != null && stockStatuses.Any())
            query = query.Where(p => stockStatuses.Contains(p.StockStatus));


        return await SortAndPageAsync(query, sort, page, pageSize);
    }

    public async Task<Dictionary<Guid, List<string>>> GetProductImageKeysAsync(List<Guid> productIds)
    {
        if (productIds == null || !productIds.Any())
            return new Dictionary<Guid, List<string>>();

        var rawData = new List<(Guid Id, string Key)>();

        foreach (var chunk in productIds.Chunk(500))
        {
            var chunkResults = await _dbContext.Set<Product>()
                .AsNoTracking()
                .Where(p => chunk.Contains(p.Id))
                .SelectMany(p => p.Images, (p, i) => new { p.Id, i.Key })
                .ToListAsync();

            var mappedChunk = chunkResults.Select(x => (x.Id, x.Key));

            rawData.AddRange(mappedChunk);
        }

        return rawData
            .GroupBy(x => x.Id)
            .ToDictionary(
                g => g.Key,
                g => g.Select(x => x.Key).ToList()
            );
    }


    public async Task AddBulkAsync(IEnumerable<Product> products)
    {
        await _dbContext.Set<Product>().AddRangeAsync(products);
    }

    public override async Task<Product?> GetByIdAsync(Guid productId)
    {
        return await _dbContext.Set<Product>()
            .Include(x => x.Images)
            .AsSplitQuery()
            .FirstOrDefaultAsync(x => x.Id == productId);
    }

    public override async Task<List<Product>> GetByIdsAsync(IEnumerable<Guid> ids)
    {
        return await _dbContext.Set<Product>()
            .Include(x => x.Images)
            .Where(x => ids.Contains(x.Id))
            .AsSplitQuery()
            .ToListAsync();
    }
}
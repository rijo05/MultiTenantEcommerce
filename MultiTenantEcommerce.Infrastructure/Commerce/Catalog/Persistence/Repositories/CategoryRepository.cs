using Microsoft.EntityFrameworkCore;
using MultiTenantEcommerce.Domain.Commerce.Catalog.Entities;
using MultiTenantEcommerce.Domain.Commerce.Catalog.Interfaces;
using MultiTenantEcommerce.Infrastructure.Persistence.Catalog.Context;
using MultiTenantEcommerce.Infrastructure.Persistence.Context;
using MultiTenantEcommerce.Infrastructure.Persistence.Repositories;
using MultiTenantEcommerce.Shared.Application;
using MultiTenantEcommerce.Shared.Application.CQRS;

namespace MultiTenantEcommerce.Infrastructure.Persistence.Catalog.Repositories;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    public CategoryRepository(CatalogDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Category?> GetByExactNameAsync(string name)
    {
        return await _dbContext.Set<Category>()
            .FirstOrDefaultAsync(c => c.Name == name);
    }

    public async Task<PaginatedList<Category>> GetFilteredAsync(
        string? name = null,
        string? description = null,
        bool? isActive = null,
        int page = 1,
        int pageSize = 20,
        SortOptions sort = SortOptions.TimeDesc)
    {
        var query = _dbContext.Set<Category>()
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(name))
            query = query.Where(p => EF.Functions.Like(p.Name, $"%{name}%"));

        if (!string.IsNullOrWhiteSpace(description))
            query = query.Where(p => EF.Functions.Like(p.Name, $"%{name}%"));

        if (isActive.HasValue)
            query = query.Where(p => p.IsActive == isActive.Value);


        return await SortAndPageAsync(query, sort, page, pageSize);
    }
}
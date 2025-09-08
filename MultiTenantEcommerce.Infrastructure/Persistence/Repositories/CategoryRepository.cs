using Microsoft.EntityFrameworkCore;
using MultiTenantEcommerce.Domain.Catalog.Entities;
using MultiTenantEcommerce.Domain.Catalog.Interfaces;
using MultiTenantEcommerce.Domain.Enums;
using MultiTenantEcommerce.Infrastructure.Persistence.Context;

namespace MultiTenantEcommerce.Infrastructure.Persistence.Repositories;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    public CategoryRepository(AppDbContext appDbContext, TenantContext tenantContext) : base(appDbContext, tenantContext) { }

    public async Task<Category?> GetByExactNameAsync(string name)
    {
        return await _appDbContext.Categories
            .FirstOrDefaultAsync(c => c.Name == name);
    }
    public async Task<List<Category>> GetFilteredAsync(
    string? name = null,
    string? description = null,
    bool? isActive = null,
    int page = 1,
    int pageSize = 20,
    SortOptions sort = SortOptions.TimeDesc)
    {
        var query = _appDbContext.Categories.AsQueryable();

        if (!string.IsNullOrWhiteSpace(name))
            query = query.Where(p => EF.Functions.Like(p.Name, $"%{name}%"));

        if (!string.IsNullOrWhiteSpace(description))
            query = query.Where(p => EF.Functions.Like(p.Name, $"%{name}%"));

        if (isActive.HasValue)
            query = query.Where(p => p.IsActive == isActive.Value);


        return await SortAndPageAsync(query, sort, page, pageSize);
    }
}

using Microsoft.EntityFrameworkCore;
using MultiTenantEcommerce.Domain.Entities;
using MultiTenantEcommerce.Domain.Enums;
using MultiTenantEcommerce.Domain.Interfaces;
using MultiTenantEcommerce.Domain.ValueObjects;
using MultiTenantEcommerce.Infrastructure.Context;

namespace MultiTenantEcommerce.Infrastructure.Repositories;

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
    SortOptions? sort = null)
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

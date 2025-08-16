using Microsoft.EntityFrameworkCore;
using MultiTenantEcommerce.Domain.Entities;
using MultiTenantEcommerce.Domain.Interfaces;

namespace MultiTenantEcommerce.Infrastructure.Repositories;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    public CategoryRepository(AppDbContext appDbContext) : base(appDbContext) { }

    public async Task<List<Category>> SearchByNameAsync(string name)
    {
        return await _appDbContext.Categories
            .Where(u => EF.Functions.Like(u.Name, $"%{name}%"))
            .ToListAsync();
    }
    public async Task<Category?> GetByExactNameAsync(string name)
    {
        return await _appDbContext.Categories
            .FirstOrDefaultAsync(c => c.Name == name);
    }
}

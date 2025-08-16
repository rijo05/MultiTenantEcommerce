using Microsoft.EntityFrameworkCore;
using MultiTenantEcommerce.Domain.Enums;
using MultiTenantEcommerce.Domain.Interfaces;

namespace MultiTenantEcommerce.Infrastructure.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly AppDbContext _appDbContext;

    public Repository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<List<T>> GetAllAsync()
    {
        return await _appDbContext.Set<T>().ToListAsync();
    }

    public async Task<T?> GetByIdAsync(Guid id)
    {
        return await _appDbContext.Set<T>().FindAsync(id);
    }
    public async Task AddAsync(T entity)
    {
        await _appDbContext.Set<T>().AddAsync(entity); ;
    }

    public async Task DeleteAsync(T entity)
    {
        _appDbContext.Set<T>().Remove(entity);
    }
    public async Task UpdateAsync(T entity)
    {
         _appDbContext.Set<T>().Update(entity);
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _appDbContext.Set<T>().AnyAsync(x => EF.Property<Guid>(x, "Id") == id);
    }

    public async Task<List<T>> GetByIdsAsync(IEnumerable<Guid> ids)
    {
        return await _appDbContext.Set<T>().Where(x => ids.Contains(EF.Property<Guid>(x, "Id"))).ToListAsync();
    }

    protected async Task<List<T>> SortAndPageAsync(IQueryable<T> query, SortOptions? sort, int page, int pageSize)
    {
        var sortProperty = sort switch
        {
            SortOptions.NameAsc or SortOptions.NameDesc => "Name",
            SortOptions.PriceAsc or SortOptions.PriceDesc => "Price",
            SortOptions.TimeAsc or SortOptions.TimeDesc => "CreatedAt",
            _ => "Name"
        };

        if (HasProperty<T>(sortProperty))
        {
            query = sort switch
            {
                SortOptions.NameAsc => query.OrderBy(p => EF.Property<object>(p, sortProperty)),
                SortOptions.NameDesc => query.OrderByDescending(p => EF.Property<object>(p, sortProperty)),
                SortOptions.PriceAsc => query.OrderBy(p => EF.Property<object>(p, sortProperty)),
                SortOptions.PriceDesc => query.OrderByDescending(p => EF.Property<object>(p, sortProperty)),
                SortOptions.TimeAsc => query.OrderBy(p => EF.Property<object>(p, sortProperty)),
                SortOptions.TimeDesc => query.OrderByDescending(p => EF.Property<object>(p, sortProperty)),
                _ => query.OrderBy(p => EF.Property<object>(p, "Name"))
            };
        }
        else
            query = query.OrderBy(p => EF.Property<object>(p, "Name"));


        var pageNumber = Math.Max(page, 1);
        var pageSizeClamped = Math.Clamp(pageSize, 1, 100);

        return await query
            .Skip((pageNumber - 1) * pageSizeClamped)
            .Take(pageSizeClamped)
            .ToListAsync();
    }
    private bool HasProperty<T>(string propertyName)
    {
        return typeof(T).GetProperty(propertyName) != null;
    }
}

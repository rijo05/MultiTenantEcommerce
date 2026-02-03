using Microsoft.EntityFrameworkCore;
using MultiTenantEcommerce.Domain.Common.Entities;
using MultiTenantEcommerce.Domain.Common.Interfaces;
using MultiTenantEcommerce.Domain.Enums;
using MultiTenantEcommerce.Infrastructure.Persistence.Context;

namespace MultiTenantEcommerce.Infrastructure.Persistence.Repositories;

public class Repository<T> : IRepository<T> where T : BaseEntity
{
    protected readonly AppDbContext _appDbContext;

    public Repository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public virtual async Task<List<T>> GetAllAsync()
    {
        return await _appDbContext.Set<T>().ToListAsync();
    }

    public virtual async Task<T?> GetByIdAsync(Guid id)
    {
        return await _appDbContext.Set<T>().FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task AddAsync(T entity)
    {
        await _appDbContext.Set<T>().AddAsync(entity);
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
        return await _appDbContext.Set<T>().AnyAsync(x => x.Id == id);
    }

    public virtual async Task<List<T>> GetByIdsAsync(IEnumerable<Guid> ids)
    {
        var distinctIds = ids.Distinct().ToList();

        if (!distinctIds.Any())
            return new List<T>();

        return await _appDbContext.Set<T>()
            .Where(x => distinctIds.Contains(x.Id))
            .ToListAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _appDbContext.SaveChangesAsync();
    }

    protected async Task<List<T>> SortAndPageAsync(IQueryable<T> query, SortOptions sort, int page, int pageSize)
    {
        var sortProperty = sort switch
        {
            SortOptions.NameAsc or SortOptions.NameDesc => "Name",
            SortOptions.PriceAsc or SortOptions.PriceDesc => "Price",
            SortOptions.TimeAsc or SortOptions.TimeDesc => "CreatedAt",
            _ => "CreatedAt"
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
                _ => query.OrderBy(p => p.CreatedAt)
            };
        }
        else
        {
            query = query.OrderBy(p => p.CreatedAt);
        }


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

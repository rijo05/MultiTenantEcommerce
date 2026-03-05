using Microsoft.EntityFrameworkCore;
using MultiTenantEcommerce.Shared.Application;
using MultiTenantEcommerce.Shared.Application.CQRS;
using MultiTenantEcommerce.Shared.Application.Interfaces;
using MultiTenantEcommerce.Shared.Domain.Abstractions;

namespace MultiTenantEcommerce.Infrastructure.Persistence.Repositories;

public class Repository<T> : IRepository<T> where T : BaseEntity
{
    protected readonly DbContext _dbContext;

    public Repository(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public virtual async Task<List<T>> GetAllAsync()
    {
        return await _dbContext.Set<T>().ToListAsync();
    }

    public virtual async Task<T?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Set<T>().FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task AddAsync(T entity)
    {
        await _dbContext.Set<T>().AddAsync(entity);
    }

    public async Task DeleteAsync(T entity)
    {
        _dbContext.Set<T>().Remove(entity);
    }

    public async Task UpdateAsync(T entity)
    {
        _dbContext.Set<T>().Update(entity);
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _dbContext.Set<T>().AnyAsync(x => x.Id == id);
    }

    public virtual async Task<List<T>> GetByIdsAsync(IEnumerable<Guid> ids)
    {
        var distinctIds = ids.Distinct().ToList();

        if (!distinctIds.Any())
            return new List<T>();

        return await _dbContext.Set<T>()
            .Where(x => distinctIds.Contains(x.Id))
            .ToListAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }

    protected async Task<PaginatedList<T>> SortAndPageAsync(IQueryable<T> query, SortOptions sort, int page, int pageSize)
    {
        var sortProperty = sort switch
        {
            SortOptions.NameAsc or SortOptions.NameDesc => "Name",
            SortOptions.PriceAsc or SortOptions.PriceDesc => "Price",
            SortOptions.TimeAsc or SortOptions.TimeDesc => "CreatedAt",
            _ => "CreatedAt"
        };

        if (HasProperty<T>(sortProperty))
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
        else
            query = query.OrderByDescending(p => EF.Property<object>(p, "CreatedAt"));

        var totalCount = await query.CountAsync();

        var pageNumber = Math.Max(page, 1);
        var pageSizeClamped = Math.Clamp(pageSize, 1, 100);

        if (totalCount == 0)
            return new PaginatedList<T>(new List<T>(), 0, pageNumber, pageSizeClamped);

        var items = await query
            .Skip((pageNumber - 1) * pageSizeClamped)
            .Take(pageSizeClamped)
            .ToListAsync();

        return new PaginatedList<T>(items, totalCount, pageNumber, pageSizeClamped);
    }

    private bool HasProperty<T>(string propertyName)
    {
        return typeof(T).GetProperty(propertyName) != null;
    }
}
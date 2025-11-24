using Microsoft.EntityFrameworkCore;
using MultiTenantEcommerce.Application.Common.Interfaces.Persistence;
using MultiTenantEcommerce.Domain.Common.Entities;
using MultiTenantEcommerce.Domain.Common.Interfaces;
using MultiTenantEcommerce.Domain.Enums;
using MultiTenantEcommerce.Domain.Tenants.Entities;
using MultiTenantEcommerce.Infrastructure.Persistence.Context;
using System.Linq.Expressions;

namespace MultiTenantEcommerce.Infrastructure.Persistence.Repositories;

public class Repository<T> : IRepository<T> where T : BaseEntity
{
    protected readonly AppDbContext _appDbContext;
    private readonly ITenantContext _tenantContext;

    public Repository(AppDbContext appDbContext, ITenantContext tenantContext)
    {
        _appDbContext = appDbContext;
        _tenantContext = tenantContext;
    }

    public async Task<List<T>> GetAllAsync()
    {
        return await _appDbContext.Set<T>().ToListAsync();
    }

    public async Task<T?> GetByIdAsync(params object[] keyValues)
    {
        var validKeyValues = CheckIfItsValid(keyValues);

        return await _appDbContext.Set<T>().FindAsync(validKeyValues);
    }

    public async Task<T?> GetByIdIncluding(Guid id, params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = _appDbContext.Set<T>();

        foreach (var include in includes)
            query = query.Include(include);

        return await query.FirstOrDefaultAsync(x => x.Id == id);
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

    public async Task<List<T>> GetByIdsAsync(IEnumerable<Guid> ids)
    {
        return await _appDbContext.Set<T>().Where(x => ids.Contains(x.Id)).ToListAsync();
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

    private object[] CheckIfItsValid(params object[] keyValues)
    {
        if (keyValues == null || keyValues.Length == 0)
            throw new ArgumentException("At least one key must be provided.");

        if (typeof(T) != typeof(Tenant))
        {
            var updatedKeyValues = new object[keyValues.Length + 1];
            updatedKeyValues[0] = _tenantContext.TenantId;
            Array.Copy(keyValues, 0, updatedKeyValues, 1, keyValues.Length);

            return updatedKeyValues;
        }

        return keyValues;
    }
}

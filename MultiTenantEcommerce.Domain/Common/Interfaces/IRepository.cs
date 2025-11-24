using MultiTenantEcommerce.Domain.Common.Entities;
using System.Linq.Expressions;

namespace MultiTenantEcommerce.Domain.Common.Interfaces;

public interface IRepository<T> where T : BaseEntity
{
    public Task<List<T>> GetAllAsync();
    public Task<T?> GetByIdAsync(params object[] keyValues);
    public Task<List<T>> GetByIdsAsync(IEnumerable<Guid> ids);
    public Task<T?> GetByIdIncluding(Guid id, params Expression<Func<T, object>>[] includes);
    public Task AddAsync(T entity);
    public Task DeleteAsync(T entity);
    public Task UpdateAsync(T entity);
    public Task<bool> ExistsAsync(Guid id);
    public Task SaveChangesAsync();
}

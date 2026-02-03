using MultiTenantEcommerce.Domain.Common.Entities;

namespace MultiTenantEcommerce.Domain.Common.Interfaces;

public interface IRepository<T> where T : BaseEntity
{
    public Task<List<T>> GetAllAsync();
    public Task<T?> GetByIdAsync(Guid id);
    public Task<List<T>> GetByIdsAsync(IEnumerable<Guid> ids);
    public Task AddAsync(T entity);
    public Task DeleteAsync(T entity);
    public Task UpdateAsync(T entity);
    public Task<bool> ExistsAsync(Guid id);
    public Task SaveChangesAsync();
}

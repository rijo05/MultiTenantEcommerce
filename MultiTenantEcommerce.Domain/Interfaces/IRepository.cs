namespace MultiTenantEcommerce.Domain.Interfaces;

public interface IRepository<T> where T : class
{
    public Task<List<T>> GetAllAsync();
    public Task<T?> GetByIdAsync(Guid id);
    public Task<List<T>> GetByIdsAsync(IEnumerable<Guid> ids);
    public Task AddAsync(T entity);
    public Task DeleteAsync(T entity);
    public Task UpdateAsync(T entity);
    public Task<bool> ExistsAsync(Guid id);
}

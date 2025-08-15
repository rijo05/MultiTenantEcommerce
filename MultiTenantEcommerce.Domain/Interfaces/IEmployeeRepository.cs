using MultiTenantEcommerce.Domain.Entities;
using MultiTenantEcommerce.Domain.ValueObjects;

namespace MultiTenantEcommerce.Domain.Interfaces;

public interface IEmployeeRepository : IRepository<Employee>
{
    public Task<Employee?> GetByEmailAsync(Email email);
    public Task<List<Employee>> GetFilteredAsync(
        string? name = null,
        string? role = null,
        string? email = null,
        bool? isActive = null,
        int page = 1,
        int pageSize = 20,
        string? sort = null);
}

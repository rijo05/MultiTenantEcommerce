using MultiTenantEcommerce.Domain.Common.Interfaces;
using MultiTenantEcommerce.Domain.Enums;
using MultiTenantEcommerce.Domain.Users.Entities;
using MultiTenantEcommerce.Domain.ValueObjects;

namespace MultiTenantEcommerce.Domain.Users.Interfaces;

public interface IEmployeeRepository : IRepository<Employee>
{
    public Task<Employee?> GetByEmailAsync(Email email);

    public Task<Employee?> GetByIdWithRolesAsync(Guid employeeId);
    public Task<Employee?> GetByEmailWithRolesAsync(Email email);
    public Task<List<Employee>> GetEmployeesByRole(
        Guid roleId,
        int page = 1,
        int pageSize = 20,
        SortOptions sort = SortOptions.TimeDesc);
    public Task<bool> HasEmployeesWithRole(Guid roleId);

    public Task<List<Employee>> GetFilteredAsync(
        string? name = null,
        string? role = null,
        string? email = null,
        bool? isActive = null,
        int page = 1,
        int pageSize = 20,
        SortOptions sort = SortOptions.TimeDesc);

    public Task<Employee?> GetOwnerOfTenant(Guid tenantId);
}

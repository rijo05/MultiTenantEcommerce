using Microsoft.EntityFrameworkCore;
using MultiTenantEcommerce.Domain.Enums;
using MultiTenantEcommerce.Domain.Users.Entities;
using MultiTenantEcommerce.Domain.Users.Interfaces;
using MultiTenantEcommerce.Domain.ValueObjects;
using MultiTenantEcommerce.Infrastructure.Persistence.Context;

namespace MultiTenantEcommerce.Infrastructure.Persistence.Repositories;
public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
{
    public EmployeeRepository(AppDbContext appDbContext, TenantContext tenantContext) : base(appDbContext, tenantContext) { }
            
    public async Task<Employee?> GetByEmailAsync(Email email)
    {
        return await _appDbContext.Employees.FirstOrDefaultAsync(u => u.Email.Value == email.Value);
    }

    public async Task<List<Employee>> GetFilteredAsync(
    string? name = null,
    string? role = null,
    string? email = null,
    bool? isActive = null,
    int page = 1,
    int pageSize = 20,
    SortOptions sort = SortOptions.TimeDesc)
    {
        var query = _appDbContext.Employees.AsQueryable();

        if (!string.IsNullOrWhiteSpace(name))
            query = query.Where(p => EF.Functions.Like(p.Name, $"%{name}%"));

        if (isActive.HasValue)
            query = query.Where(p => p.IsActive == isActive.Value);

        if (!string.IsNullOrWhiteSpace(email))
            query = query.Where(p => p.Email != null && EF.Functions.Like(p.Email.Value, $"%{email}%"));

        if (!string.IsNullOrWhiteSpace(role))
            query = query.Where(p => p.Role != null && EF.Functions.Like(p.Role.roleName.ToString(), $"%{role}%"));


        return await SortAndPageAsync(query, sort, page, pageSize);
    }
}

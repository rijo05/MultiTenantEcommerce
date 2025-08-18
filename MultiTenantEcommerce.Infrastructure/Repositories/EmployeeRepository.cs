using Microsoft.EntityFrameworkCore;
using MultiTenantEcommerce.Domain.Entities;
using MultiTenantEcommerce.Domain.Enums;
using MultiTenantEcommerce.Domain.Interfaces;
using MultiTenantEcommerce.Domain.ValueObjects;
using MultiTenantEcommerce.Infrastructure.Context;

namespace MultiTenantEcommerce.Infrastructure.Repositories;
public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
{
    public EmployeeRepository(AppDbContext appDbContext) : base(appDbContext) { }
            
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
    SortOptions? sort = null)
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

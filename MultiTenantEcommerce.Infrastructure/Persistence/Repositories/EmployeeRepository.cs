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
        var query = _appDbContext.Employees
            .Include(e => e.EmployeeRoles)
            .ThenInclude(er => er.Role)
            .ThenInclude(r => r.Permissions)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(name))
            query = query.Where(p => EF.Functions.Like(p.Name, $"%{name}%"));

        if (isActive.HasValue)
            query = query.Where(p => p.IsActive == isActive.Value);

        if (!string.IsNullOrWhiteSpace(email))
            query = query.Where(p => p.Email != null && EF.Functions.Like(p.Email.Value, $"%{email}%"));

        if (!string.IsNullOrWhiteSpace(role))
            query = query.Where(p => p.EmployeeRoles
                .Any(x => EF.Functions.Like(x.Role.Name, $"%{role}%")));


        return await SortAndPageAsync(query, sort, page, pageSize);
    }

    public async Task<Employee?> GetByIdWithRolesAsync(Guid employeeId)
    {
        return await _appDbContext.Employees
            .Include(x => x.EmployeeRoles)
                .ThenInclude(x => x.Role)
                    .ThenInclude(x => x.Permissions)
            .FirstOrDefaultAsync(x => x.Id == employeeId);
    }

    public async Task<Employee?> GetByEmailWithRolesAsync(Email email)
    {
        return await _appDbContext.Employees
            .Include(x => x.EmployeeRoles)
                .ThenInclude(x => x.Role)
                    .ThenInclude(x => x.Permissions)
            .FirstOrDefaultAsync(x => x.Email.Value == email.Value);
    }

    public async Task<List<Employee>> GetEmployeesByRole(
        Guid roleId,
        int page = 1,
        int pageSize = 20,
        SortOptions sort = SortOptions.TimeDesc)
    {
        var query = _appDbContext.Employees
            .Include(x => x.EmployeeRoles)
                .ThenInclude(x => x.Role)
                    .ThenInclude(x => x.Permissions)
            .Where(x => x.EmployeeRoles.Any(x => x.RoleId == roleId));

        return await SortAndPageAsync(query, sort, page, pageSize);
    }

    public async Task<bool> HasEmployeesWithRole(Guid roleId)
    {
        return await _appDbContext.Employees.AnyAsync(e => e.EmployeeRoles.Any(er => er.RoleId == roleId));
    }

    public async Task<Employee?> GetOwnerOfTenant(Guid tenantId)
    {
        return await _appDbContext.Employees
            .IgnoreQueryFilters()
            .Include(e => e.EmployeeRoles)
                .ThenInclude(er => er.Role)
            .Where(e => e.TenantId == tenantId
                        && e.EmployeeRoles.Any(er => er.Role.Name.ToLower() == "owner"))
            .FirstOrDefaultAsync();
    }
}

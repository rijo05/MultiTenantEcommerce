using Microsoft.EntityFrameworkCore;
using MultiTenantEcommerce.Domain.Enums;
using MultiTenantEcommerce.Domain.Users.Entities;
using MultiTenantEcommerce.Domain.Users.Interfaces;
using MultiTenantEcommerce.Domain.ValueObjects;
using MultiTenantEcommerce.Infrastructure.Persistence.Context;

namespace MultiTenantEcommerce.Infrastructure.Persistence.Repositories;
public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
{
    public EmployeeRepository(AppDbContext appDbContext) : base(appDbContext) { }

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
            .AsNoTracking()
            .Include(e => e.EmployeeRoles)
            .AsSplitQuery()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(name))
            query = query.Where(p => EF.Functions.Like(p.Name, $"%{name}%"));

        if (isActive.HasValue)
            query = query.Where(p => p.IsActive == isActive.Value);

        if (!string.IsNullOrWhiteSpace(email))
            query = query.Where(p => p.Email != null && EF.Functions.Like(p.Email.Value, $"%{email}%"));

        if (!string.IsNullOrWhiteSpace(role))
        {
            var roleIds = _appDbContext.Roles
                .Where(r => EF.Functions.Like(r.Name, $"%{role}%"))
                .Select(r => r.Id);

            query = query.Where(e => e.EmployeeRoles.Any(er => roleIds.Contains(er.RoleId)));
        }

        return await SortAndPageAsync(query, sort, page, pageSize);
    }

    public async Task<Employee?> GetByEmail(Email email)
    {
        return await _appDbContext.Employees
            .Include(x => x.EmployeeRoles)
            .AsSplitQuery()
            .FirstOrDefaultAsync(x => x.Email.Value == email.Value);
    }

    public override async Task<Employee?> GetByIdAsync(Guid id)
    {
        return await _appDbContext.Employees
            .Include(x => x.EmployeeRoles)
            .AsSplitQuery()
            .FirstOrDefaultAsync(x => x.Id == id);
    }
    public async Task<List<Employee>> GetEmployeesByRole(
        Guid roleId,
        int page = 1,
        int pageSize = 20,
        SortOptions sort = SortOptions.TimeDesc)
    {
        var query = _appDbContext.Employees
            .Include(x => x.EmployeeRoles)
            .AsSplitQuery()
            .Where(x => x.EmployeeRoles.Any(er => er.RoleId == roleId));

        return await SortAndPageAsync(query, sort, page, pageSize);
    }

    public async Task<bool> HasEmployeesWithRole(Guid roleId)
    {
        return await _appDbContext.Employees
            .AnyAsync(e => e.EmployeeRoles.Any(er => er.RoleId == roleId));
    }

    public async Task<Employee?> GetOwnerOfTenant(Guid tenantId)
    {
        var ownerRoleId = await _appDbContext.Roles
                .IgnoreQueryFilters()
                .Where(r => r.TenantId == tenantId && r.Name == SystemRoles.Owner)
                .Select(r => r.Id)
                .FirstOrDefaultAsync();

        if (ownerRoleId == Guid.Empty) return null;

        return await _appDbContext.Employees
            .IgnoreQueryFilters()
            .Include(e => e.EmployeeRoles)
            .Where(e => e.TenantId == tenantId
                        && e.EmployeeRoles.Any(er => er.RoleId == ownerRoleId))
            .AsSplitQuery()
            .FirstOrDefaultAsync();
    }
}

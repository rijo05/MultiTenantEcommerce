using Microsoft.EntityFrameworkCore;
using MultiTenantEcommerce.Domain.Entities;
using MultiTenantEcommerce.Domain.Enums;
using MultiTenantEcommerce.Domain.Interfaces;
using MultiTenantEcommerce.Domain.ValueObjects;
using MultiTenantEcommerce.Infrastructure.Context;
using System.Data;

namespace MultiTenantEcommerce.Infrastructure.Repositories;
public class CustomerRepository : Repository<Customer>, ICustomerRepository
{
    public CustomerRepository(AppDbContext appDbContext, TenantContext tenantContext) : base(appDbContext, tenantContext) { }

    public async Task<Customer?> GetByEmailAsync(Email email)
    {
        return await _appDbContext.Customers.FirstOrDefaultAsync(x => x.Email.Value == email.Value);
    }

    public async Task<Customer?> GetByPhoneNumberAsync(PhoneNumber phoneNumber)
    {
        return await _appDbContext.Customers.FirstOrDefaultAsync(x => x.PhoneNumber.Number == phoneNumber.Number);
    }

    public async Task<List<Customer>> GetFilteredAsync(
    string? name = null, 
    string? email = null, 
    bool? isActive = null, 
    string? phoneNumber = null, 
    int page = 1, 
    int pageSize = 20, 
    SortOptions? sort = null)
    {
        var query = _appDbContext.Customers.AsQueryable();

        if (!string.IsNullOrWhiteSpace(name))
            query = query.Where(p => EF.Functions.Like(p.Name, $"%{name}%"));

        if (isActive.HasValue)
            query = query.Where(p => p.IsActive == isActive.Value);

        if (!string.IsNullOrWhiteSpace(email))
            query = query.Where(p => p.Email != null && EF.Functions.Like(p.Email.Value, $"%{email}%"));

        if (!string.IsNullOrWhiteSpace(phoneNumber))
            query = query.Where(p => p.PhoneNumber != null && EF.Functions.Like(p.PhoneNumber.Number, $"%{phoneNumber}%"));


        return await SortAndPageAsync(query, sort, page, pageSize);
    }
}

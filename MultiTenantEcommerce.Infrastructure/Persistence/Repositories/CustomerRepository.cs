using Microsoft.EntityFrameworkCore;
using MultiTenantEcommerce.Domain.Enums;
using MultiTenantEcommerce.Domain.Users.Entities;
using MultiTenantEcommerce.Domain.Users.Interfaces;
using MultiTenantEcommerce.Domain.ValueObjects;
using MultiTenantEcommerce.Infrastructure.Persistence.Context;
using System.Data;

namespace MultiTenantEcommerce.Infrastructure.Persistence.Repositories;
public class CustomerRepository : Repository<Customer>, ICustomerRepository
{
    public CustomerRepository(AppDbContext appDbContext) : base(appDbContext) { }

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
    string? phoneNumber = null,
    bool? isActive = null,
    int page = 1,
    int pageSize = 20,
    SortOptions sort = SortOptions.TimeDesc)
    {
        var query = _appDbContext.Customers
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(name))
            query = query.Where(p => EF.Functions.Like(p.Name, $"%{name}%"));

        if (isActive.HasValue)
            query = query.Where(p => p.IsActive == isActive.Value);

        if (!string.IsNullOrWhiteSpace(email))
            query = query.Where(p => p.Email != null && EF.Functions.Like(p.Email.Value, $"%{email}%"));

        //talvez tirar #########
        if (!string.IsNullOrWhiteSpace(phoneNumber))
            query = query.Where(p => p.PhoneNumber != null && EF.Functions.Like(p.PhoneNumber.Number, $"%{phoneNumber}%"));


        return await SortAndPageAsync(query, sort, page, pageSize);
    }
}

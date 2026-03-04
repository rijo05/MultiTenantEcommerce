using Microsoft.EntityFrameworkCore;
using MultiTenantEcommerce.Domain.Commerce.Customers.Entities;
using MultiTenantEcommerce.Domain.Commerce.Customers.Interfaces;
using MultiTenantEcommerce.Infrastructure.Persistence.Context;
using MultiTenantEcommerce.Shared.Application;
using MultiTenantEcommerce.Shared.Application.CQRS;
using MultiTenantEcommerce.Shared.Domain.ValueObjects;

namespace MultiTenantEcommerce.Infrastructure.Persistence.Repositories;

public class CustomerRepository : Repository<Customer>, ICustomerRepository
{
    public CustomerRepository(AppDbContext appDbContext) : base(appDbContext)
    {
    }

    public async Task<Customer?> GetByEmailAsync(Email email)
    {
        return await _appDbContext.Customers.FirstOrDefaultAsync(x => x.Email.Value == email.Value);
    }

    public async Task<bool> CheckEmailInUse(Email email)
    {
        return await _appDbContext.Customers.AnyAsync(x => x.Email.Value == email.Value);
    }

    public async Task<PaginatedList<Customer>> GetFilteredAsync(
        string? name = null,
        string? email = null,
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
            query = query.Where(p => EF.Functions.Like(p.FirstName, $"%{name}%"));

        //if (isActive.HasValue)
        //    query = query.Where(p => p.IsActive == isActive.Value);

        if (!string.IsNullOrWhiteSpace(email))
            query = query.Where(p => p.Email != null && EF.Functions.Like(p.Email.Value, $"%{email}%"));


        return await SortAndPageAsync(query, sort, page, pageSize);
    }
}
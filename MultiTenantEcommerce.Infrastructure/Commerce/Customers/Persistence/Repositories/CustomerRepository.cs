using Microsoft.EntityFrameworkCore;
using MultiTenantEcommerce.Domain.Commerce.Customers.Entities;
using MultiTenantEcommerce.Domain.Commerce.Customers.Interfaces;
using MultiTenantEcommerce.Infrastructure.Commerce.Customers.Persistence.Context;
using MultiTenantEcommerce.Infrastructure.Persistence.Context;
using MultiTenantEcommerce.Infrastructure.Persistence.Repositories;
using MultiTenantEcommerce.Shared.Application;
using MultiTenantEcommerce.Shared.Application.CQRS;
using MultiTenantEcommerce.Shared.Domain.ValueObjects;

namespace MultiTenantEcommerce.Infrastructure.Commerce.Customers.Persistence.Repositories;

public class CustomerRepository : Repository<Customer>, ICustomerRepository
{
    public CustomerRepository(CustomerDbContext appDbContext) : base(appDbContext)
    {
    }

    public async Task<Customer?> GetByEmailAsync(Email email)
    {
        return await _dbContext.Set<Customer>().FirstOrDefaultAsync(x => x.Email.Value == email.Value);
    }

    public async Task<bool> CheckEmailInUse(Email email)
    {
        return await _dbContext.Set<Customer>().AnyAsync(x => x.Email.Value == email.Value);
    }

    public async Task<PaginatedList<Customer>> GetFilteredAsync(
        string? name = null,
        string? email = null,
        bool? isActive = null,
        int page = 1,
        int pageSize = 20,
        SortOptions sort = SortOptions.TimeDesc)
    {
        var query = _dbContext.Set<Customer>()
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
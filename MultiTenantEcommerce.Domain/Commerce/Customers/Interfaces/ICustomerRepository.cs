using MultiTenantEcommerce.Domain.Commerce.Customers.Entities;
using MultiTenantEcommerce.Shared.Application;
using MultiTenantEcommerce.Shared.Application.CQRS;
using MultiTenantEcommerce.Shared.Domain.ValueObjects;
using MultiTenantEcommerce.Shared.Infrastructure.Persistence;

namespace MultiTenantEcommerce.Domain.Commerce.Customers.Interfaces;

public interface ICustomerRepository : IRepository<Customer>
{
    public Task<Customer?> GetByEmailAsync(Email email);

    public Task<bool> CheckEmailInUse(Email email);

    public Task<PaginatedList<Customer>> GetFilteredAsync(
        string? name = null,
        string? email = null,
        bool? isActive = null,
        int page = 1,
        int pageSize = 20,
        SortOptions sort = SortOptions.TimeDesc);
}
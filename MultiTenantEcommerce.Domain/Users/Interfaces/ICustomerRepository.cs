using MultiTenantEcommerce.Domain.Common.Interfaces;
using MultiTenantEcommerce.Domain.Enums;
using MultiTenantEcommerce.Domain.Users.Entities;
using MultiTenantEcommerce.Domain.ValueObjects;

namespace MultiTenantEcommerce.Domain.Users.Interfaces;
public interface ICustomerRepository : IRepository<Customer>
{
    public Task<Customer?> GetByEmailAsync(Email email);
    public Task<Customer?> GetByPhoneNumberAsync(PhoneNumber phoneNumber);

    public Task<List<Customer>> GetFilteredAsync(
    string? name = null,
    string? email = null,
    string? phoneNumber = null,
    bool? isActive = null,
    int page = 1,
    int pageSize = 20,
    SortOptions sort = SortOptions.TimeDesc);
}


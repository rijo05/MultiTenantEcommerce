using MultiTenantEcommerce.Domain.Platform.Billing.Entities;
using MultiTenantEcommerce.Shared.Application.Interfaces;

namespace MultiTenantEcommerce.Domain.Platform.Billing.Interfaces;

public interface IAccountRepository : IRepository<Account>
{
    Task<Account?> GetByOwnerIdAsync(Guid userId);

    Task<Account?> GetByStripeCustomerIdAsync(string stripeCustomerId);
}
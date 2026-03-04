using MultiTenantEcommerce.Domain.Commerce.Sales.ShoppingCart.Entities;
using MultiTenantEcommerce.Shared.Infrastructure.Persistence;

namespace MultiTenantEcommerce.Domain.Commerce.Sales.ShoppingCart.Interfaces;

public interface ICartRepository : IRepository<Cart>
{
    Task<Cart?> GetByCustomerIdAsync(Guid customerId);

    Task<Cart?> GetActiveCartAsync(Guid? customerId, Guid? anonymousId);

}
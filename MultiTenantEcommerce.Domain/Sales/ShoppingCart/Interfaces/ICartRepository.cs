using MultiTenantEcommerce.Domain.Common.Interfaces;
using MultiTenantEcommerce.Domain.Sales.ShoppingCart.Entities;

namespace MultiTenantEcommerce.Domain.Sales.ShoppingCart.Interfaces;
public interface ICartRepository : IRepository<Cart>
{
    Task<Cart?> GetByCustomerIdAsync(Guid customerId);
}

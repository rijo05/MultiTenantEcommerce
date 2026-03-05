using MultiTenantEcommerce.Application.Commerce.Customers.Interfaces;
using MultiTenantEcommerce.Infrastructure.Commerce.Customers.Persistence.Context;
using MultiTenantEcommerce.Infrastructure.Commerce.Shipping.Persistence.Context;
using MultiTenantEcommerce.Infrastructure.Shared.Messaging;
using MultiTenantEcommerce.Infrastructure.Shared.Persistence;
using MultiTenantEcommerce.Shared.Application.Interfaces;

namespace MultiTenantEcommerce.Infrastructure.Commerce.Shipping.Persistence.Repositories;

public class ShippingUnitOfWork : UnitOfWork<ShippingDbContext>, ICustomerUnitOfWork
{
    public ShippingUnitOfWork(
        ShippingDbContext dbContext,
        EventDispatcher eventDispatcher,
        IIntegrationEventPublisher integrationEventPublisher)
        : base(dbContext, eventDispatcher, integrationEventPublisher)
    {
    }
}

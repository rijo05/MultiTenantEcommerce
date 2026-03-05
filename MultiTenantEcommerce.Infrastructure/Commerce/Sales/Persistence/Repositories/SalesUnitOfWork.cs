using MultiTenantEcommerce.Application.Commerce.Inventory.Interfaces;
using MultiTenantEcommerce.Infrastructure.Commerce.Inventory.Persistence.Context;
using MultiTenantEcommerce.Infrastructure.Commerce.Sales.Persistence.Context;
using MultiTenantEcommerce.Infrastructure.Messaging;
using MultiTenantEcommerce.Infrastructure.Persistence.Repositories;
using MultiTenantEcommerce.Shared.Application.Interfaces;

namespace MultiTenantEcommerce.Infrastructure.Commerce.Sales.Persistence.Repositories;

public class SalesUnitOfWork : UnitOfWork<SalesDbContext>, IInventoryUnitOfWork
{
    public SalesUnitOfWork(
        SalesDbContext dbContext,
        EventDispatcher eventDispatcher,
        IIntegrationEventPublisher integrationEventPublisher)
        : base(dbContext, eventDispatcher, integrationEventPublisher)
    {
    }
}

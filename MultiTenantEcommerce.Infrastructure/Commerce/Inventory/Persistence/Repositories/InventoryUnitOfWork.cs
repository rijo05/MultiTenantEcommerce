using MultiTenantEcommerce.Application.Commerce.Inventory.Interfaces;
using MultiTenantEcommerce.Infrastructure.Commerce.Inventory.Persistence.Context;
using MultiTenantEcommerce.Infrastructure.Messaging;
using MultiTenantEcommerce.Infrastructure.Persistence.Repositories;
using MultiTenantEcommerce.Shared.Application.Interfaces;

namespace MultiTenantEcommerce.Infrastructure.Commerce.Inventory.Persistence.Repositories;
public class InventoryUnitOfWork : UnitOfWork<InventoryDbContext>, IInventoryUnitOfWork
{
    public InventoryUnitOfWork(
        InventoryDbContext dbContext,
        EventDispatcher eventDispatcher,
        IIntegrationEventPublisher integrationEventPublisher)
        : base(dbContext, eventDispatcher, integrationEventPublisher)
    {
    }
}

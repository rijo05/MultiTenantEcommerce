using MultiTenantEcommerce.Application.Commerce.Catalog.Interfaces;
using MultiTenantEcommerce.Infrastructure.Persistence.Catalog.Context;
using MultiTenantEcommerce.Infrastructure.Platform.Billing.Persistence.Context;
using MultiTenantEcommerce.Infrastructure.Shared.Messaging;
using MultiTenantEcommerce.Infrastructure.Shared.Persistence;
using MultiTenantEcommerce.Shared.Application.Interfaces;

namespace MultiTenantEcommerce.Infrastructure.Platform.Billing.Persistence.Repositories;

public class BillingUnitOfWork : UnitOfWork<BillingDbContext>, ICatalogUnitOfWork
{
    public BillingUnitOfWork(
        BillingDbContext dbContext,
        EventDispatcher eventDispatcher,
        IIntegrationEventPublisher integrationEventPublisher)
        : base(dbContext, eventDispatcher, integrationEventPublisher)
    {
    }
}

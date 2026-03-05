using MultiTenantEcommerce.Application.Commerce.Catalog.Interfaces;
using MultiTenantEcommerce.Infrastructure.Messaging;
using MultiTenantEcommerce.Infrastructure.Persistence.Repositories;
using MultiTenantEcommerce.Infrastructure.Platform.Tenancy.Persistence.Context;
using MultiTenantEcommerce.Shared.Application.Interfaces;

namespace MultiTenantEcommerce.Infrastructure.Platform.Tenancy.Persistence.Repositories;
public class TenancyUnitOfWork : UnitOfWork<TenancyDbContext>, ICatalogUnitOfWork
{
    public TenancyUnitOfWork(
        TenancyDbContext dbContext,
        EventDispatcher eventDispatcher,
        IIntegrationEventPublisher integrationEventPublisher)
        : base(dbContext, eventDispatcher, integrationEventPublisher)
    {
    }
}

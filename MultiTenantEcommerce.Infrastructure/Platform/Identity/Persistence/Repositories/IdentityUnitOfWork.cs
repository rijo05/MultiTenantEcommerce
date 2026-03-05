using MultiTenantEcommerce.Application.Commerce.Catalog.Interfaces;
using MultiTenantEcommerce.Infrastructure.Messaging;
using MultiTenantEcommerce.Infrastructure.Persistence.Repositories;
using MultiTenantEcommerce.Infrastructure.Platform.Identity.Persistence.Context;
using MultiTenantEcommerce.Shared.Application.Interfaces;
namespace MultiTenantEcommerce.Infrastructure.Platform.Identity.Persistence.Repositories;

public class IdentityUnitOfWork : UnitOfWork<IdentityDbContext>, ICatalogUnitOfWork
{
    public IdentityUnitOfWork(
        IdentityDbContext dbContext,
        EventDispatcher eventDispatcher,
        IIntegrationEventPublisher integrationEventPublisher)
        : base(dbContext, eventDispatcher, integrationEventPublisher)
    {
    }
}

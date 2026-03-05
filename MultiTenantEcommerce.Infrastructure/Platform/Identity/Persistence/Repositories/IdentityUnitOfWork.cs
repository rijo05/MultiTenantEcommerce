using MultiTenantEcommerce.Application.Commerce.Catalog.Interfaces;
using MultiTenantEcommerce.Infrastructure.Platform.Identity.Persistence.Context;
using MultiTenantEcommerce.Infrastructure.Shared.Messaging;
using MultiTenantEcommerce.Infrastructure.Shared.Persistence;
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

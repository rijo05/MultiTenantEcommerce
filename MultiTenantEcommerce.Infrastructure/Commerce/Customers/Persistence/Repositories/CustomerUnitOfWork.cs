using MultiTenantEcommerce.Application.Commerce.Catalog.Interfaces;
using MultiTenantEcommerce.Application.Commerce.Customers.Interfaces;
using MultiTenantEcommerce.Infrastructure.Commerce.Customers.Persistence.Context;
using MultiTenantEcommerce.Infrastructure.Persistence.Catalog.Context;
using MultiTenantEcommerce.Infrastructure.Persistence.Repositories;
using MultiTenantEcommerce.Infrastructure.Shared.Messaging;
using MultiTenantEcommerce.Shared.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Infrastructure.Commerce.Customers.Persistence.Repositories;

public class CustomerUnitOfWork : UnitOfWork<CustomerDbContext>, ICustomerUnitOfWork
{
    public CustomerUnitOfWork(
        CustomerDbContext dbContext,
        EventDispatcher eventDispatcher,
        IIntegrationEventPublisher integrationEventPublisher)
        : base(dbContext, eventDispatcher, integrationEventPublisher)
    {
    }
}

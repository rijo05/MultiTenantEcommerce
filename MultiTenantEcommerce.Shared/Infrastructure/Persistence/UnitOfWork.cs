using Microsoft.EntityFrameworkCore;
using MultiTenantEcommerce.Infrastructure.Persistence.Context;
using MultiTenantEcommerce.Infrastructure.Persistence.Transaction;
using MultiTenantEcommerce.Shared.Application.Interfaces;
using MultiTenantEcommerce.Shared.Domain.Exceptions;

namespace MultiTenantEcommerce.Infrastructure.Persistence.Repositories;

public class UnitOfWork<TContext> : IUnitOfWork where TContext : ModuleDbContext
{
    private readonly TContext _dbContext;
    private readonly IEventDispatcher _eventDispatcher;
    private readonly IIntegrationEventPublisher _integrationEventPublisher;

    public UnitOfWork(TContext dbContext, IEventDispatcher eventDispatcher, IIntegrationEventPublisher integrationEventPublisher)
    {
        _dbContext = dbContext;
        _eventDispatcher = eventDispatcher;
        _integrationEventPublisher = integrationEventPublisher;
    }

    public async Task<ITransaction> BeginTransactionAsync()
    {
        var transaction = await _dbContext.Database.BeginTransactionAsync();
        return new EfCoreTransaction(transaction);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _dbContext.SaveChangesAsync();
    }

    public async Task<int> CommitAsync()
    {
        var result = 0;

        try
        {
            while (true)
            {
                var domainEvents = _dbContext.GetAllDomainEvents();
                if (!domainEvents.Any()) break;

                _dbContext.ClearAllDomainEvents();

                foreach (var domainEvent in domainEvents)
                {
                    await _eventDispatcher.DispatchSync(domainEvent);
                }
            }

            var integrationEvents = _integrationEventPublisher.GetAllEvents();

            if (integrationEvents.Any())
            {
                var outboxMessages = integrationEvents.Select(evt => new OutboxEvent(evt)).ToList();

                await _dbContext.OutboxEvents.AddRangeAsync(outboxMessages);

                _integrationEventPublisher.ClearEvents();
            }

            result = await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new ConcurrencyException("Concurrency conflict while saving changes", ex);
        }

        return result;
    }
}
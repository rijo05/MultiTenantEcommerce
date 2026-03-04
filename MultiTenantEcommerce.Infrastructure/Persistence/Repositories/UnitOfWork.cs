using Microsoft.EntityFrameworkCore;
using MultiTenantEcommerce.Infrastructure.Messaging;
using MultiTenantEcommerce.Infrastructure.Outbox;
using MultiTenantEcommerce.Infrastructure.Persistence.Context;
using MultiTenantEcommerce.Infrastructure.Persistence.Transaction;
using MultiTenantEcommerce.Shared.Application.Events;
using MultiTenantEcommerce.Shared.Domain.Exceptions;
using MultiTenantEcommerce.Shared.Infrastructure.Persistence;

namespace MultiTenantEcommerce.Infrastructure.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private readonly EventDispatcher _eventDispatcher;
    private readonly IIntegrationEventPublisher _integrationEventPublisher;

    public UnitOfWork(AppDbContext context, EventDispatcher eventDispatcher, IIntegrationEventPublisher integrationEventPublisher)
    {
        _context = context;
        _eventDispatcher = eventDispatcher;
        _integrationEventPublisher = integrationEventPublisher;
    }

    public async Task<ITransaction> BeginTransactionAsync()
    {
        var transaction = await _context.Database.BeginTransactionAsync();
        return new EfCoreTransaction(transaction);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task<int> CommitAsync()
    {
        var result = 0;

        try
        {
            while (true)
            {
                var domainEvents = _context.GetAllDomainEvents();
                if (!domainEvents.Any()) break;

                _context.ClearAllDomainEvents();

                foreach (var domainEvent in domainEvents)
                {
                    await _eventDispatcher.DispatchSync(domainEvent);
                }
            }

            var integrationEvents = _integrationEventPublisher.GetAllEvents();

            if (integrationEvents.Any())
            {
                var outboxMessages = integrationEvents.Select(evt => new OutboxEvent(evt)).ToList();

                await _context.OutboxEvents.AddRangeAsync(outboxMessages);

                _integrationEventPublisher.ClearEvents();
            }

            result = await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new ConcurrencyException("Concurrency conflict while saving changes", ex);
        }

        return result;
    }
}
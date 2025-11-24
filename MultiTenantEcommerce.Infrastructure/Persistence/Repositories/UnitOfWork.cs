using MultiTenantEcommerce.Application.Common.Exceptions;
using MultiTenantEcommerce.Application.Common.Interfaces.Persistence;
using MultiTenantEcommerce.Infrastructure.Outbox;
using MultiTenantEcommerce.Infrastructure.Persistence.Context;
using MultiTenantEcommerce.Infrastructure.Persistence.Transaction;
using System.Data.Entity.Infrastructure;

namespace MultiTenantEcommerce.Infrastructure.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
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
            var events = _context.GetAllDomainEvents();
            _context.ClearAllDomainEvents();

            foreach (var domainEvent in events)
            {
                var outboxMessage = new OutboxEvent(domainEvent);

                await _context.OutboxEvents.AddAsync(outboxMessage);
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

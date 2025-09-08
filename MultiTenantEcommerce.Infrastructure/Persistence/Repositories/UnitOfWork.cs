using MediatR;
using MultiTenantEcommerce.Application.Common.Exceptions;
using MultiTenantEcommerce.Application.Common.Interfaces.Persistence;
using MultiTenantEcommerce.Infrastructure.Persistence.Context;
using MultiTenantEcommerce.Infrastructure.Persistence.Transaction;
using System.Data.Entity.Infrastructure;

namespace MultiTenantEcommerce.Infrastructure.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private readonly IMediator _mediator;

    public UnitOfWork(AppDbContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public async Task<ITransaction> BeginTransactionAsync()
    {
        var transaction = await _context.Database.BeginTransactionAsync();
        return new EfCoreTransaction(transaction);
    }

    public async Task<int> CommitAsync()
    {
        var result = 0;

        try
        {
            result = await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new ConcurrencyException("Concurrency conflict while saving changes", ex);
        }

        var events = _context.GetAllDomainEvents();
        _context.ClearAllDomainEvents();

        foreach (var domainEvent in events)
            await _mediator.Publish(domainEvent);

        return result;
    }
}

using MediatR;
using MultiTenantEcommerce.Domain.Common.Interfaces;
using MultiTenantEcommerce.Infrastructure.Persistence.Context;

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

    public async Task<int> CommitAsync()
    {
        var result = await _context.SaveChangesAsync();

        var events = _context.GetAllDomainEvents();
        _context.ClearAllDomainEvents();

        foreach (var domainEvent in events)
            await _mediator.Publish(domainEvent);

        return result;
    }
}

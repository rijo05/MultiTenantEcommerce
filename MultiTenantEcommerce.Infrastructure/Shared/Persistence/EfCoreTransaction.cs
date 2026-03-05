using Microsoft.EntityFrameworkCore.Storage;
using MultiTenantEcommerce.Shared.Application.Interfaces;

namespace MultiTenantEcommerce.Infrastructure.Shared.Persistence;

public class EfCoreTransaction : ITransaction
{
    private readonly IDbContextTransaction _transaction;

    public EfCoreTransaction(IDbContextTransaction transaction)
    {
        _transaction = transaction;
    }

    public Task CommitAsync()
    {
        return _transaction.CommitAsync();
    }

    public Task RollbackAsync()
    {
        return _transaction.RollbackAsync();
    }

    public ValueTask DisposeAsync()
    {
        return _transaction.DisposeAsync();
    }
}
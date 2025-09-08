using Microsoft.EntityFrameworkCore.Storage;
using MultiTenantEcommerce.Application.Common.Interfaces.Persistence;

namespace MultiTenantEcommerce.Infrastructure.Persistence.Transaction;
public class EfCoreTransaction : ITransaction
{
    private readonly IDbContextTransaction _transaction;
    public EfCoreTransaction(IDbContextTransaction transaction) => _transaction = transaction;

    public Task CommitAsync() => _transaction.CommitAsync();
    public Task RollbackAsync() => _transaction.RollbackAsync();
    public ValueTask DisposeAsync() => _transaction.DisposeAsync();
}

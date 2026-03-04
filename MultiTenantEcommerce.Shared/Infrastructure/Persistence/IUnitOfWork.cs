namespace MultiTenantEcommerce.Shared.Infrastructure.Persistence;

public interface IUnitOfWork
{
    Task<int> CommitAsync();

    Task<ITransaction> BeginTransactionAsync();
}
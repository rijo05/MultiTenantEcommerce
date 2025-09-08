namespace MultiTenantEcommerce.Application.Common.Interfaces.Persistence;

public interface IUnitOfWork
{
    Task<int> CommitAsync();

    Task<ITransaction> BeginTransactionAsync();
}

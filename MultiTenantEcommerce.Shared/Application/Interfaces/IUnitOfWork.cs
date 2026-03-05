namespace MultiTenantEcommerce.Shared.Application.Interfaces;

public interface IUnitOfWork
{
    Task<int> CommitAsync();

    Task<ITransaction> BeginTransactionAsync();
}
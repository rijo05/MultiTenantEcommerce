namespace MultiTenantEcommerce.Domain.Interfaces;

public interface IUnitOfWork
{
    Task<int> CommitAsync();
}

namespace MultiTenantEcommerce.Domain.Common.Interfaces;

public interface IUnitOfWork
{
    Task<int> CommitAsync();
}

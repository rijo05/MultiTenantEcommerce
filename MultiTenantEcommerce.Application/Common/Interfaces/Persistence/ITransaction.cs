namespace MultiTenantEcommerce.Application.Common.Interfaces.Persistence;
public interface ITransaction : IAsyncDisposable
{
    Task CommitAsync();
    Task RollbackAsync();
}

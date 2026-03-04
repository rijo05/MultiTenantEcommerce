namespace MultiTenantEcommerce.Shared.Infrastructure.Persistence;

public interface ITransaction : IAsyncDisposable
{
    Task CommitAsync();
    Task RollbackAsync();
}
namespace MultiTenantEcommerce.Application.Common.Interfaces.Persistence;
public interface IEventBus
{
    public Task PublishAsync(string RoutingKey, string Message);
}

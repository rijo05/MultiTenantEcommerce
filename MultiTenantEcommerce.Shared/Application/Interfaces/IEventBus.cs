namespace MultiTenantEcommerce.Shared.Application.Interfaces;

public interface IEventBus
{
    public Task PublishAsync(string content, string eventType, int priority);
}
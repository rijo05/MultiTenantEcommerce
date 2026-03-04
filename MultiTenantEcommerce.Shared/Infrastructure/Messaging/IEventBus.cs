namespace MultiTenantEcommerce.Shared.Infrastructure.Messaging;

public interface IEventBus
{
    public Task PublishAsync(string content, string eventType, int priority);
}
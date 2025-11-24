namespace MultiTenantEcommerce.Infrastructure.Messaging;
public class NonCriticalConsumer : RabbitMqConsumer
{
    public NonCriticalConsumer(EventDispatcher dispatcher,
        RabbitMqConnectionManager connectionManager)
        : base(dispatcher, connectionManager, "noncritical-domain-events-queue", "noncritical.#")
    {
    }
}

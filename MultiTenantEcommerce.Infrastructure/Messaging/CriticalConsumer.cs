namespace MultiTenantEcommerce.Infrastructure.Messaging;
public class CriticalConsumer : RabbitMqConsumer
{
    public CriticalConsumer(EventDispatcher dispatcher,
        RabbitMqConnectionManager connectionManager)
        : base(dispatcher, connectionManager, "critical-domain-events-queue", "critical.#")
    {
    }
}

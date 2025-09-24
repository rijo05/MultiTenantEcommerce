using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Infrastructure.Messaging;
public class NonCriticalConsumer : RabbitMqConsumer
{
    public NonCriticalConsumer(EventDispatcher dispatcher, 
        RabbitMqConnectionManager connectionManager) 
        : base(dispatcher, connectionManager, "noncritical-domain-events-queue", "noncritical.#")
    {
    }
}

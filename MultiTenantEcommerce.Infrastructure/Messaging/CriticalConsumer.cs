using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Infrastructure.Messaging;
public class CriticalConsumer : RabbitMqConsumer
{
    public CriticalConsumer(EventDispatcher dispatcher, 
        RabbitMqConnectionManager connectionManager) 
        : base(dispatcher, connectionManager, "critical-domain-events-queue", "critical.#")
    {
    }
}

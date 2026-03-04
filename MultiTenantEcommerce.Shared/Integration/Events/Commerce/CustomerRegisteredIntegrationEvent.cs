using MultiTenantEcommerce.Shared.Infrastructure.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Shared.Integration.Events.Commerce;

public record CustomerRegisteredIntegrationEvent(
    Guid TenantId,
    Guid CustomerId,
    string CustomerEmail,
    string CustomerName) : IntegrationEvent(TenantId);

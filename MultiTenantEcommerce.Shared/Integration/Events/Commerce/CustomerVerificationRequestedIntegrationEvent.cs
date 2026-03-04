using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiTenantEcommerce.Shared.Infrastructure.Messaging;

namespace MultiTenantEcommerce.Shared.Integration.Events.Commerce;

[HighPriority]
public record CustomerVerificationRequestedIntegrationEvent(
    Guid TenantId,
    Guid CustomerId,
    string CustomerEmail,
    string CustomerName,
    string Token) : IntegrationEvent(TenantId);

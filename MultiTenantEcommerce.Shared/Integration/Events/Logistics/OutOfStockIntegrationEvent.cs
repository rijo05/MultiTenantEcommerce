using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Shared.Integration.Events.Logistics;

public record OutOfStockIntegrationEvent(
    Guid TenantId,
    Guid ProductId) : IntegrationEvent(TenantId);

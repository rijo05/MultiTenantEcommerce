using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Shared.Integration.Events.Logistics;

public record LowStockIntegrationEvent(
    Guid TenantId,
    Guid ProductId,
    int CurrentQuantity,
    int MinimumQuantity) : IntegrationEvent(TenantId);

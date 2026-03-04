using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Shared.Integration.Events.Logistics;
public record InStockIntegrationEvent(
    Guid TenantId, 
    Guid ProductId) : IntegrationEvent(TenantId);

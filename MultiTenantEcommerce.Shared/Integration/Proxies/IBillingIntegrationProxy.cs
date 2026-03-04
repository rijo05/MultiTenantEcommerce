using MultiTenantEcommerce.Shared.Integration.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Shared.Integration.Proxies;
public interface IBillingIntegrationProxy
{
    Task<PlanValidationResultDTO> ValidatePlanAndPriceAsync(Guid planId, string stripePriceId);

    Task<TenantPlanLimitsDTO?> GetPlanLimitsAsync(Guid planId);
}

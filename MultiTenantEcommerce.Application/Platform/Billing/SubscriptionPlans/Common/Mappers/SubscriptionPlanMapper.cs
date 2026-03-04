using MultiTenantEcommerce.Application.Platform.Billing.SubscriptionPlans.Common.DTOs;
using MultiTenantEcommerce.Domain.Platform.Billing.Entities;

namespace MultiTenantEcommerce.Application.Platform.Billing.SubscriptionPlans.Common.Mappers;

public class SubscriptionPlanMapper
{
    public SubscriptionPlanResponseDTO ToSubscriptionPlanResponseDTO(SubscriptionPlan plan)
    {
        return new SubscriptionPlanResponseDTO
        {
            Id = plan.Id,
            Name = plan.Name,
            PlanLimits = plan.PlanLimits
        };
    }

    public List<SubscriptionPlanResponseDTO> ToSubscriptionPlanResponseDTOList(List<SubscriptionPlan> plan)
    {
        return plan.Select(x => ToSubscriptionPlanResponseDTO(x)).ToList();
    }
}
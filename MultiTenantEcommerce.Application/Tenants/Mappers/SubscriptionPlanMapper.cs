using MultiTenantEcommerce.Application.Tenants.DTOs.Plans;
using MultiTenantEcommerce.Domain.Tenants.Entities;

namespace MultiTenantEcommerce.Application.Tenants.Mappers;
public class SubscriptionPlanMapper
{
    public SubscriptionPlanResponseDTO ToSubscriptionPlanResponseDTO(SubscriptionPlan plan)
    {
        return new SubscriptionPlanResponseDTO()
        {
            Id = plan.Id,
            Name = plan.Name,
            PlanLimits = plan.PlanLimits,
            Price = plan.Price.Value,
            TransactionFee = plan.TransactionFee
        };
    }

    public List<SubscriptionPlanResponseDTO> ToSubscriptionPlanResponseDTOList(List<SubscriptionPlan> plan)
    {
        return plan.Select(x => ToSubscriptionPlanResponseDTO(x)).ToList();
    }
}

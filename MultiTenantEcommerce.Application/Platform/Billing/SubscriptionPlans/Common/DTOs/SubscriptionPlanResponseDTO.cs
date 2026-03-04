using MultiTenantEcommerce.Domain.Platform.Billing.Entities;

namespace MultiTenantEcommerce.Application.Platform.Billing.SubscriptionPlans.Common.DTOs;

public class SubscriptionPlanResponseDTO
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public decimal Price { get; init; }
    public PlanLimits PlanLimits { get; init; }
}
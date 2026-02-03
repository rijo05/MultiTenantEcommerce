using MultiTenantEcommerce.Domain.Tenants.Entities;

namespace MultiTenantEcommerce.Application.Tenants.DTOs.Plans;
public class SubscriptionPlanResponseDTO
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public decimal Price { get; init; }
    public PlanLimits PlanLimits { get; init; }
}

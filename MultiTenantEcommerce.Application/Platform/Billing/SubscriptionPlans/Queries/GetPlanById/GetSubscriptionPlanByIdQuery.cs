using MultiTenantEcommerce.Application.Platform.Billing.SubscriptionPlans.Common.DTOs;

namespace MultiTenantEcommerce.Application.Platform.Billing.SubscriptionPlans.Queries.GetPlanById;

public record GetSubscriptionPlanByIdQuery(Guid PlanId) : IQuery<SubscriptionPlanResponseDTO>;
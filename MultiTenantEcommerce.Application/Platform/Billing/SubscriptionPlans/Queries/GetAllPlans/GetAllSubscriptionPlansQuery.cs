using MultiTenantEcommerce.Application.Platform.Billing.SubscriptionPlans.Common.DTOs;

namespace MultiTenantEcommerce.Application.Platform.Billing.SubscriptionPlans.Queries.GetAllPlans;

public record GetAllSubscriptionPlansQuery : IQuery<List<SubscriptionPlanResponseDTO>>;
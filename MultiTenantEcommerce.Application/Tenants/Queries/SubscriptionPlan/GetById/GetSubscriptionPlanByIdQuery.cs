using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Tenants.DTOs.Plans;

namespace MultiTenantEcommerce.Application.Tenants.Queries.SubscriptionPlan.GetById;
public record GetSubscriptionPlanByIdQuery(Guid PlanId) : IQuery<SubscriptionPlanResponseDTO>;

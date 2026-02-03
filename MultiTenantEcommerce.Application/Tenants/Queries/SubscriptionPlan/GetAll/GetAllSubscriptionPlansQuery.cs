using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Tenants.DTOs.Plans;

namespace MultiTenantEcommerce.Application.Tenants.Queries.SubscriptionPlan.GetAll;
public record GetAllSubscriptionPlansQuery() : IQuery<List<SubscriptionPlanResponseDTO>>;

using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Tenants.DTOs.Plans;
using MultiTenantEcommerce.Application.Tenants.Mappers;
using MultiTenantEcommerce.Domain.Tenants.Interfaces;

namespace MultiTenantEcommerce.Application.Tenants.Queries.SubscriptionPlan.GetById;
public class GetSubscriptionPlanByIdQueryHandler : IQueryHandler<GetSubscriptionPlanByIdQuery, SubscriptionPlanResponseDTO>
{
    private readonly ISubscriptionPlanRepository _subscriptionPlanRepository;
    private readonly SubscriptionPlanMapper _subscriptionPlanMapper;

    public GetSubscriptionPlanByIdQueryHandler(ISubscriptionPlanRepository subscriptionPlanRepository,
        SubscriptionPlanMapper subscriptionPlanMapper)
    {
        _subscriptionPlanRepository = subscriptionPlanRepository;
        _subscriptionPlanMapper = subscriptionPlanMapper;
    }

    public async Task<SubscriptionPlanResponseDTO> Handle(GetSubscriptionPlanByIdQuery request, CancellationToken cancellationToken)
    {
        var plan = await _subscriptionPlanRepository.GetByIdAsync(request.PlanId)
            ?? throw new Exception("Plan doesnt exist");

        return _subscriptionPlanMapper.ToSubscriptionPlanResponseDTO(plan);
    }
}

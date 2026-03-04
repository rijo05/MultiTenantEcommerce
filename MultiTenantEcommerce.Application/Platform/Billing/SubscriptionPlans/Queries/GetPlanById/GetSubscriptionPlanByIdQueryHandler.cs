using MultiTenantEcommerce.Application.Platform.Billing.SubscriptionPlans.Common.DTOs;
using MultiTenantEcommerce.Application.Platform.Billing.SubscriptionPlans.Common.Mappers;
using MultiTenantEcommerce.Domain.Platform.Billing.Interfaces;

namespace MultiTenantEcommerce.Application.Platform.Billing.SubscriptionPlans.Queries.GetPlanById;

public class
    GetSubscriptionPlanByIdQueryHandler : IQueryHandler<GetSubscriptionPlanByIdQuery, SubscriptionPlanResponseDTO>
{
    private readonly SubscriptionPlanMapper _subscriptionPlanMapper;
    private readonly ISubscriptionPlanRepository _subscriptionPlanRepository;

    public GetSubscriptionPlanByIdQueryHandler(ISubscriptionPlanRepository subscriptionPlanRepository,
        SubscriptionPlanMapper subscriptionPlanMapper)
    {
        _subscriptionPlanRepository = subscriptionPlanRepository;
        _subscriptionPlanMapper = subscriptionPlanMapper;
    }

    public async Task<SubscriptionPlanResponseDTO> Handle(GetSubscriptionPlanByIdQuery request,
        CancellationToken cancellationToken)
    {
        var plan = await _subscriptionPlanRepository.GetByIdAsync(request.PlanId)
                   ?? throw new Exception("Plan doesnt exist");

        return _subscriptionPlanMapper.ToSubscriptionPlanResponseDTO(plan);
    }
}
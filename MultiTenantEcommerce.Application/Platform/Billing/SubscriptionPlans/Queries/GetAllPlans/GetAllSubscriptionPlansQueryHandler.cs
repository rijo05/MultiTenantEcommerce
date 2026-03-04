using MultiTenantEcommerce.Application.Platform.Billing.SubscriptionPlans.Common.DTOs;
using MultiTenantEcommerce.Application.Platform.Billing.SubscriptionPlans.Common.Mappers;
using MultiTenantEcommerce.Domain.Platform.Billing.Interfaces;

namespace MultiTenantEcommerce.Application.Platform.Billing.SubscriptionPlans.Queries.GetAllPlans;

public class
    GetAllSubscriptionPlansQueryHandler : IQueryHandler<GetAllSubscriptionPlansQuery, List<SubscriptionPlanResponseDTO>>
{
    private readonly SubscriptionPlanMapper _subscriptionPlanMapper;
    private readonly ISubscriptionPlanRepository _subscriptionPlanRepository;

    public GetAllSubscriptionPlansQueryHandler(ISubscriptionPlanRepository subscriptionPlanRepository,
        SubscriptionPlanMapper subscriptionPlanMapper)
    {
        _subscriptionPlanRepository = subscriptionPlanRepository;
        _subscriptionPlanMapper = subscriptionPlanMapper;
    }

    public async Task<List<SubscriptionPlanResponseDTO>> Handle(GetAllSubscriptionPlansQuery request,
        CancellationToken cancellationToken)
    {
        var plans = await _subscriptionPlanRepository.GetAllAsync();

        return _subscriptionPlanMapper.ToSubscriptionPlanResponseDTOList(plans);
    }
}
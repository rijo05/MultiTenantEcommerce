using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Tenants.DTOs.Plans;
using MultiTenantEcommerce.Application.Tenants.Mappers;
using MultiTenantEcommerce.Domain.Tenants.Interfaces;

namespace MultiTenantEcommerce.Application.Tenants.Queries.SubscriptionPlan.GetAll;
public class GetAllSubscriptionPlansQueryHandler : IQueryHandler<GetAllSubscriptionPlansQuery, List<SubscriptionPlanResponseDTO>>
{
    private readonly ISubscriptionPlanRepository _subscriptionPlanRepository;
    private readonly SubscriptionPlanMapper _subscriptionPlanMapper;

    public GetAllSubscriptionPlansQueryHandler(ISubscriptionPlanRepository subscriptionPlanRepository,
        SubscriptionPlanMapper subscriptionPlanMapper)
    {
        _subscriptionPlanRepository = subscriptionPlanRepository;
        _subscriptionPlanMapper = subscriptionPlanMapper;
    }

    public async Task<List<SubscriptionPlanResponseDTO>> Handle(GetAllSubscriptionPlansQuery request, CancellationToken cancellationToken)
    {
        var plans = await _subscriptionPlanRepository.GetAllAsync();

        return _subscriptionPlanMapper.ToSubscriptionPlanResponseDTOList(plans);
    }
}

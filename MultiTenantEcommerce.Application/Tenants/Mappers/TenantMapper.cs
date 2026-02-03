using MultiTenantEcommerce.Application.Tenants.DTOs.Tenant;
using MultiTenantEcommerce.Domain.Tenants.Entities;

namespace MultiTenantEcommerce.Application.Tenants.Mappers;

public class TenantMapper
{
    private readonly SubscriptionPlanMapper _subscriptionPlanMapper;

    public TenantMapper(SubscriptionPlanMapper subscriptionPlanMapper)
    {
        _subscriptionPlanMapper = subscriptionPlanMapper;
    }

    public TenantResponseDTO ToTenantResponseDTO(Tenant tenant)
    {
        return new TenantResponseDTO
        {
            CompanyName = tenant.Name,
            Email = tenant.Email.Value,
            Shipping = tenant.ShippingProviders.Select(s => new ShippingProviderConfigDTO
            {
                Carrier = s.Carrier.ToString(),
                IsActive = s.IsActive
            }).ToList(),
            Status = tenant.Subscription.Status.ToString(),
            CurrentPeriodStart = tenant.Subscription.CurrentPeriodStart,
            CurrentPeriodEnd = tenant.Subscription.CurrentPeriodEnd,
            CreatedAt = tenant.CreatedAt,
            SubscriptionPlan = _subscriptionPlanMapper.ToSubscriptionPlanResponseDTO(tenant.Subscription.Plan)
        };
    }

    public List<TenantResponseDTO> ToTenantResponseDTOList(IEnumerable<Tenant> tenants)
    {
        return tenants.Select(x => ToTenantResponseDTO(x)).ToList();
    }
}

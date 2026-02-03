using MultiTenantEcommerce.Application.Tenants.DTOs.Plans;

namespace MultiTenantEcommerce.Application.Tenants.DTOs.Tenant;
public class TenantResponseDTO
{
    public string CompanyName { get; init; }
    public string Email { get; init; }
    public List<ShippingProviderConfigDTO> Shipping { get; init; }
    public string Status { get; init; }
    public DateTime CurrentPeriodStart { get; init; }
    public DateTime CurrentPeriodEnd { get; init; }
    public DateTime CreatedAt { get; init; }
    public SubscriptionPlanResponseDTO SubscriptionPlan { get; init; }
}

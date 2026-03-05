using Amazon.Runtime.Internal.Util;
using HtmlAgilityPack;
using MultiTenantEcommerce.Domain.Platform.Billing.Interfaces;
using MultiTenantEcommerce.Shared.Integration.DTOs;
using MultiTenantEcommerce.Shared.Integration.Proxies;

namespace MultiTenantEcommerce.Infrastructure.Platform.Billing;
public class BillingIntegrationProxy : IBillingIntegrationProxy
{
    private readonly ISubscriptionPlanRepository _planRepository;

    public BillingIntegrationProxy(ISubscriptionPlanRepository planRepository)
    {
        _planRepository = planRepository;
    }

    public async Task<PlanValidationResultDTO> ValidatePlanAndPriceAsync(Guid planId, string stripePriceId)
    {
        var plan = await _planRepository.GetByIdAsync(planId);

        if (plan == null)
            return new PlanValidationResultDTO(false, null, "O plano selecionado não existe.");

        if (!plan.IsActive)
            return new PlanValidationResultDTO(false, null, "O plano selecionado já não está ativo.");

        var price = plan.Prices.FirstOrDefault(x => x.StripePriceId == stripePriceId);

        if (price == null || !price.IsActive)
            return new PlanValidationResultDTO(false, null, "O preço selecionado não é válido para este plano.");

        return new PlanValidationResultDTO(true, price.Id, null);
    }

    public async Task<TenantPlanLimitsDTO?> GetPlanLimitsAsync(Guid planId)
    {
        var plan = await _planRepository.GetByIdAsync(planId);

        if (plan == null) return null;

        return new TenantPlanLimitsDTO(plan.PlanLimits.MaxProducts, 
            plan.PlanLimits.MaxTenantMembers, 
            plan.PlanLimits.MaxStorageBytes, 
            false, 
            plan.PlanLimits.TransactionFee, 
            plan.PlanLimits.AcessToWebhooks);
    }
}

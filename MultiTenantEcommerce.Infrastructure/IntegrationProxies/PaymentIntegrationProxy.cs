using MultiTenantEcommerce.Application.Commerce.Sales.Orders.Interfaces;
using MultiTenantEcommerce.Domain.Commerce.Sales.Orders.Enums;
using MultiTenantEcommerce.Shared.Integration.DTOs;
using MultiTenantEcommerce.Shared.Integration.Proxies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Infrastructure.IntegrationProxies;
public class PaymentIntegrationProxy : IPaymentIntegrationProxy
{
    private readonly IPaymentProviderFactory _paymentProviderFactory;
    private readonly ITenantIntegrationProxy _tenantProxy;
    public async Task<PaymentResultDTO> CreatePaymentSessionAsync(Guid paymentId, Guid orderId, decimal amount, Guid tenantId)
    {
        var tenantPlan = await _tenantProxy.GetTenantPlanLimitsAsync(tenantId);
        var transactionFee = tenantPlan.Subscription.Plan.PlanLimits.TransactionFee;

        var provider = _paymentProviderFactory.GetProvider(PaymentMethod.Stripe);

        return await provider.CreatePaymentAsync(
            paymentId, orderId, amount, tenantId.ToString(), transactionFee);
    }

    public Task<string?> GetPaymentReferenceByOrderIdAsync(Guid orderId)
    {
        throw new NotImplementedException();
    }

    public Task<Dictionary<Guid, string>> GetPaymentReferencesByOrderIdsAsync(IEnumerable<Guid> orderIds)
    {
        throw new NotImplementedException();
    }
}

using MultiTenantEcommerce.Shared.Integration.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Shared.Integration.Proxies;
public interface IPaymentIntegrationProxy
{
    Task<Dictionary<Guid, string>> GetPaymentReferencesByOrderIdsAsync(IEnumerable<Guid> orderIds);
    Task<string?> GetPaymentReferenceByOrderIdAsync(Guid orderId);
    Task<PaymentResultDTO> CreatePaymentSessionAsync(
        Guid paymentId,
        Guid orderId,
        decimal amount,
        Guid tenantId);
}


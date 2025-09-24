using MultiTenantEcommerce.Application.Payment.OrderPayment.DTOs;
using MultiTenantEcommerce.Application.Payment.OrderPayment.Interfaces;
using Stripe;
using Stripe.Checkout;

namespace MultiTenantEcommerce.Infrastructure.Payments;
public class StripePaymentProvider : IPaymentProvider
{
    public async Task<PaymentResultDTO> CreatePaymentAsync(Guid PaymentId, Domain.Sales.Orders.Entities.Order order, string tenantPaymentProviderAccountId)
    {
        var domain = "http://localhost:4242";

        var lineItems = order.Items.Select(item => new SessionLineItemOptions
        {
            PriceData = new SessionLineItemPriceDataOptions
            {
                UnitAmount = (long)(item.UnitPrice.Value * 100),
                Currency = "eur",
                ProductData = new SessionLineItemPriceDataProductDataOptions
                {
                    Name = item.ProductName
                }
            },
            Quantity = item.Quantity.Value
        }).ToList();

        var shippingOptions = new List<SessionShippingOptionOptions>
        {
            new SessionShippingOptionOptions
            {
                ShippingRateData = new SessionShippingOptionShippingRateDataOptions
                {
                    DisplayName = "CTT",
                    FixedAmount = new SessionShippingOptionShippingRateDataFixedAmountOptions
                    {
                        Amount = 299, // 2.99€
                        Currency = "eur"
                    },
                    DeliveryEstimate = new SessionShippingOptionShippingRateDataDeliveryEstimateOptions
                    {
                        Minimum = new SessionShippingOptionShippingRateDataDeliveryEstimateMinimumOptions
                        { Unit = "day", Value = 2 },
                        Maximum = new SessionShippingOptionShippingRateDataDeliveryEstimateMaximumOptions
                        { Unit = "day", Value = 7 }
                    }
                }
            }
        };

        var options = new SessionCreateOptions
        {
            ClientReferenceId = order.CustomerId.ToString(),
            LineItems = lineItems,
            Mode = "payment",
            Currency = "eur",
            ShippingOptions = shippingOptions,
            SuccessUrl = domain + "/success.html",
            CancelUrl = domain + "/cancel.html",
            Metadata = new Dictionary<string, string>
            {
                { "PaymentId", PaymentId.ToString() },
                { "OrderId", order.Id.ToString() }
            }
        };

        var requestOptions = new RequestOptions
        {
            ApiKey = "",
            StripeAccount = tenantPaymentProviderAccountId,
        };

        var service = new SessionService();
        var session = await service.CreateAsync(options, requestOptions);

        return new PaymentResultDTO { PaymentURL = session.Url, PaymentId = session.Id };
    }
}

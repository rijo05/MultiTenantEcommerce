using MultiTenantEcommerce.Application.Payment.OrderPayment.DTOs;
using MultiTenantEcommerce.Application.Payment.OrderPayment.Interfaces;
using MultiTenantEcommerce.Application.Shipping.DTOs;
using MultiTenantEcommerce.Domain.Sales.Orders.Entities;
using Stripe;
using Stripe.Checkout;

namespace MultiTenantEcommerce.Infrastructure.Payments;
public class StripePaymentProvider : IPaymentProvider
{
    public async Task<PaymentResultDTO> CreatePaymentAsync(Guid PaymentId, Order order, ShippingQuoteDTO shipping, string tenantPaymentProviderAccountId)
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
                    DisplayName = shipping.Carrier.ToString(),
                    FixedAmount = new SessionShippingOptionShippingRateDataFixedAmountOptions
                    {
                        Amount = (long)shipping.Price * 100,
                        Currency = "eur"
                    },
                    DeliveryEstimate = new SessionShippingOptionShippingRateDataDeliveryEstimateOptions
                    {
                        Minimum = new SessionShippingOptionShippingRateDataDeliveryEstimateMinimumOptions
                        { Unit = "day", Value = (long)shipping.MinTransit.TotalDays },
                        Maximum = new SessionShippingOptionShippingRateDataDeliveryEstimateMaximumOptions
                        { Unit = "day", Value = (long)shipping.MaxTransit.TotalDays }
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

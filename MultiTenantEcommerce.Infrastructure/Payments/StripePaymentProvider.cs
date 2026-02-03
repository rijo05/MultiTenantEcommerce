using Microsoft.Extensions.Configuration;
using MultiTenantEcommerce.Application.Payment.OrderPayment.DTOs;
using MultiTenantEcommerce.Application.Payment.OrderPayment.Interfaces;
using MultiTenantEcommerce.Application.Shipping.DTOs;
using MultiTenantEcommerce.Domain.Sales.Orders.Entities;
using Stripe;
using Stripe.Checkout;

namespace MultiTenantEcommerce.Infrastructure.Payments;
public class StripePaymentProvider : IPaymentProvider
{
    private readonly string _apiKey = "";

    public StripePaymentProvider(IConfiguration configuration)
    {
        _apiKey = configuration["Stripe:ApiKey"];
        StripeConfiguration.ApiKey = _apiKey;
    }

    public async Task<PaymentResultDTO> CreatePaymentAsync(Guid PaymentId,
        Order order,
        ShippingQuoteDTO shipping,
        string connectedAccountId,
        decimal applicationFeeAmount)
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

        var options = new SessionCreateOptions
        {
            ClientReferenceId = order.CustomerId.ToString(),
            LineItems = lineItems,
            Mode = "payment",
            Currency = "eur",

            ShippingOptions = new List<SessionShippingOptionOptions>
            {
                new SessionShippingOptionOptions
                {
                    ShippingRateData = new SessionShippingOptionShippingRateDataOptions
                    {
                        DisplayName = shipping.Carrier.ToString(),
                        FixedAmount = new SessionShippingOptionShippingRateDataFixedAmountOptions
                        {
                            Amount = (long)(shipping.Price * 100),
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
            },

            SuccessUrl = $"{domain}/checkout/success?session_id={{CHECKOUT_SESSION_ID}}",
            CancelUrl = $"{domain}/checkout/cancel",

            // Metadados para reconciliação no Webhook
            Metadata = new Dictionary<string, string>
            {
                { "PaymentId", PaymentId.ToString() },
                { "OrderId", order.Id.ToString() },
                { "TenantId", order.TenantId.ToString() }
            },


            PaymentIntentData = new SessionPaymentIntentDataOptions
            {
                ApplicationFeeAmount = applicationFeeAmount > 0 ? (long)(applicationFeeAmount * 100) : null,
            }
        };

        var requestOptions = new RequestOptions
        {
            ApiKey = _apiKey,
            StripeAccount = connectedAccountId
        };

        var service = new SessionService();
        var session = await service.CreateAsync(options, requestOptions);

        return new PaymentResultDTO
        {
            PaymentURL = session.Url,
            PaymentId = session.Id
        };
    }

    public async Task<string> CreateCustomerAsync(string name, string email, Guid tenantId)
    {
        var options = new CustomerCreateOptions
        {
            Name = name,
            Email = email,
            Metadata = new Dictionary<string, string>
            {
                { "TenantId", tenantId.ToString() }
            }
        };

        var service = new CustomerService();
        var customer = await service.CreateAsync(options);
        return customer.Id; // "cus_123..."
    }

    public async Task<string> CreateCheckoutSessionAsync(
        string stripeCustomerId,
        string stripePriceId,
        string successUrl,
        string cancelUrl)
    {
        var options = new SessionCreateOptions
        {
            Customer = stripeCustomerId,
            Mode = "subscription",
            LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    Price = stripePriceId,
                    Quantity = 1
                }
            },
            SuccessUrl = successUrl,
            CancelUrl = cancelUrl,
            SubscriptionData = new SessionSubscriptionDataOptions
            {
                Metadata = new Dictionary<string, string>() { { "Type", "SaaS_Subscription" } }
            }
        };

        var service = new SessionService();
        var session = await service.CreateAsync(options);

        return session.Url;
    }

    public async Task<string> CreatePortalSessionAsync(string stripeCustomerId, string returnUrl)
    {
        var options = new Stripe.BillingPortal.SessionCreateOptions
        {
            Customer = stripeCustomerId,
            ReturnUrl = returnUrl,
        };
        var service = new Stripe.BillingPortal.SessionService();
        var session = await service.CreateAsync(options);
        return session.Url;
    }
}

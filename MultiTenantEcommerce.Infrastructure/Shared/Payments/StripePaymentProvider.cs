using Microsoft.Extensions.Configuration;
using MultiTenantEcommerce.Application.Commerce.Sales.Orders.Interfaces;
using MultiTenantEcommerce.Application.Commerce.Shipping.Common.DTOs;
using MultiTenantEcommerce.Domain.Commerce.Sales.Orders.Entities;
using MultiTenantEcommerce.Shared.Integration.DTOs;
using Stripe;
using Stripe.Checkout;

namespace MultiTenantEcommerce.Infrastructure.Shared.Payments;

public class StripePaymentProvider : IPaymentProvider
{
    private readonly string _apiKey = "";

    public StripePaymentProvider(IConfiguration configuration)
    {
        _apiKey = configuration["Stripe:ApiKey"];
        StripeConfiguration.ApiKey = _apiKey;
    }

    #region CUSTOMER PAGA ORDER
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
                UnitAmount = item.UnitPrice.ToLong() * 100,
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
            ExpiresAt = DateTime.UtcNow + TimeSpan.FromMinutes(10),
            LineItems = lineItems,
            Mode = "payment",
            Currency = "eur",

            ShippingOptions = new List<SessionShippingOptionOptions>
            {
                new()
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

            Metadata = new Dictionary<string, string>
            {
                { "PaymentId", PaymentId.ToString() },
                { "OrderId", order.Id.ToString() },
                { "TenantId", order.TenantId.ToString() }
            },


            PaymentIntentData = new SessionPaymentIntentDataOptions
            {
                ApplicationFeeAmount = applicationFeeAmount > 0 ? (long)(applicationFeeAmount * 100) : null,
                CaptureMethod = "manual",
                Metadata = new Dictionary<string, string>
                {
                    { "OrderId", order.Id.ToString() }
                }
            }
        };

        var requestOptions = new RequestOptions
        {
            ApiKey = _apiKey,
            StripeAccount = connectedAccountId,
            IdempotencyKey = PaymentId.ToString(),
        };

        var service = new SessionService();
        var session = await service.CreateAsync(options, requestOptions);

        return new PaymentResultDTO(session.Url, session.Id);
    }

    #endregion

    #region USER PAGA SUBSCRICAO
    //Pagar a primeira subscricao / Depois do trial
    public async Task<string> CreateCheckoutSessionAsync(
        Guid tenantId,
        string stripeCustomerId,
        string stripePriceId,
        string successUrl,
        string cancelUrl,
        DateTime trialEndsAt)
    {
        var subscriptionData = new SessionSubscriptionDataOptions
        {
            Metadata = new Dictionary<string, string>
        {
            { "Type", "SaaS_Subscription" },
            { "TenantId", tenantId.ToString() },
        }
        };

        if ((trialEndsAt - DateTime.UtcNow).TotalHours > 48)
        {
            subscriptionData.TrialEnd = trialEndsAt;
        }

        var options = new SessionCreateOptions
        {
            Customer = stripeCustomerId,
            Mode = "subscription",
            LineItems = new List<SessionLineItemOptions>
        {
            new()
            {
                Price = stripePriceId,
                Quantity = 1
            }
        },
            SuccessUrl = successUrl,
            CancelUrl = cancelUrl,
            SubscriptionData = subscriptionData
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
            ReturnUrl = returnUrl
        };
        var service = new Stripe.BillingPortal.SessionService();
        var session = await service.CreateAsync(options);
        return session.Url;
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

    #endregion

    #region USER RECEBE DINHEIRO
    public async Task<string> CreateConnectedAccountAsync(string email, string companyName)
    {
        var options = new AccountCreateOptions
        {
            Type = "express", // O modelo "Express" é o melhor para SaaS. A Stripe trata do UI.
            Email = email,
            Company = new AccountCompanyOptions { Name = companyName },
            Capabilities = new AccountCapabilitiesOptions
            {
                CardPayments = new AccountCapabilitiesCardPaymentsOptions { Requested = true },
                Transfers = new AccountCapabilitiesTransfersOptions { Requested = true },
            },
        };

        var service = new AccountService();
        var account = await service.CreateAsync(options);

        // Devolve o "acct_1234..." para tu guardares no teu Tenant.StripeAccountId
        return account.Id;
    }

    // 2. Gera o Link para ele preencher o IBAN e dados fiscais
    public async Task<string> CreateConnectOnboardingLinkAsync(string stripeAccountId, string refreshUrl, string returnUrl)
    {
        var options = new AccountLinkCreateOptions
        {
            Account = stripeAccountId,
            RefreshUrl = refreshUrl, // Se o link expirar, a Stripe manda-o para aqui para pedir um novo
            ReturnUrl = returnUrl,   // Se ele preencher tudo com sucesso, volta para o teu Dashboard
            Type = "account_onboarding",
        };

        var service = new AccountLinkService();
        var link = await service.CreateAsync(options);

        return link.Url; // O Frontend redireciona o utilizador para este URL!
    }
    #endregion
}
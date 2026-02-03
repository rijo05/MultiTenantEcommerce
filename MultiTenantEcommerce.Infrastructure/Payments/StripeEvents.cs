namespace MultiTenantEcommerce.Infrastructure.Payments;
public static class StripeEvents
{
    //CHECKOUT
    public const string CheckoutSessionCompleted = "checkout.session.completed";
    public const string CheckoutSessionExpired = "checkout.session.expired";

    //SUBSCRICOES
    public const string InvoicePaid = "invoice.paid";
    public const string InvoicePaymentFailed = "invoice.payment_failed";

    //CONNECT INICIAL
    public const string AccountUpdated = "account.updated";

    //PLANOS
    public const string ProductCreated = "product.created";
    public const string ProductUpdated = "product.updated";
    public const string ProductDeleted = "product.deleted";

    //PRICES
    public const string PriceCreated = "price.created";
    public const string PriceUpdated = "price.updated";
    public const string PriceDeleted = "price.deleted";
}

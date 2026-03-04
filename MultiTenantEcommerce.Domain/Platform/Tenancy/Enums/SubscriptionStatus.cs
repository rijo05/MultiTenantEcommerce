namespace MultiTenantEcommerce.Domain.Platform.Tenancy.Enums;

public enum SubscriptionStatus
{
    Trial, 
    Suspended, //trial acabou
    Active, // Tudo ok
    PastDue, // Pagamento falhou (inicia a contagem dos 30 dias)
    Canceled, // Acabou
}
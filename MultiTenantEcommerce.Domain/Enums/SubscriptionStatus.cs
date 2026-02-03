namespace MultiTenantEcommerce.Domain.Enums;
public enum SubscriptionStatus
{
    Incomplete, // Criado mas ainda não pagou a 1ª vez
    Active,     // Tudo ok
    PastDue,    // Pagamento falhou (inicia a contagem dos 30 dias)
    Canceled,   // Acabou
    Unpaid      // O Stripe desistiu de cobrar
}
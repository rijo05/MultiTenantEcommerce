using MultiTenantEcommerce.Domain.Commerce.Sales.Orders.Enums;
using MultiTenantEcommerce.Shared.Domain.Abstractions;
using MultiTenantEcommerce.Shared.Domain.ValueObjects;

namespace MultiTenantEcommerce.Domain.Commerce.Sales.Orders.Entities;

public class OrderPayment : TenantBase
{
    public Guid CustomerId { get; private set; }
    public Guid OrderId { get; private set; }

    public Money Amount { get; private set; }

    public PaymentStatus Status { get; private set; }
    public PaymentMethod PaymentMethod { get; private set; }

    public string? TransactionId { get; private set; }
    public string? StripeSessionId { get; private set; }
    public string? StripeSessionURL { get; private set; }

    public DateTime? CompletedAt { get; private set; }
    public DateTime? CancelledAt { get; private set; }
    public DateTime? FailedAt { get; private set; }

    public string? FailureReason { get; private set; }
    public string? Metadata { get; private set; }

    private OrderPayment()
    {
    }

    internal OrderPayment(Guid tenantId,
        Guid customerId,
        Guid orderId,
        Money amount)
        : base(tenantId)
    {
        CustomerId = customerId;
        OrderId = orderId;
        Status = PaymentStatus.AwaitingPayment;
        PaymentMethod = PaymentMethod.Stripe;
        Amount = amount;
    }

    internal void MarkAsCompleted(string transactionId)
    {
        if (string.IsNullOrWhiteSpace(transactionId))
            throw new ArgumentException("Transaction ID é obrigatório.");

        if (Status == PaymentStatus.Completed) return;

        if (Status == PaymentStatus.Failed)
            throw new Exception($"Transição inválida: O pagamento está no estado {Status}.");

        Status = PaymentStatus.Completed;
        TransactionId = transactionId;
        CompletedAt = DateTime.UtcNow;
        SetUpdatedAt();
    }

    internal void MarkAsFailed(string? reason = null)
    {
        if (Status == PaymentStatus.Failed) return;

        if (Status == PaymentStatus.Completed)
            throw new Exception("Um pagamento concluído não pode ser marcado como falhado.");

        Status = PaymentStatus.Failed;
        FailureReason = reason;
        FailedAt = DateTime.UtcNow;
        SetUpdatedAt();
    }

    internal void UpdateMetadata(string metadata)
    {
        Metadata = metadata;
        SetUpdatedAt();
    }

    internal void UpdateStripeSessionId(string sessionId)
    {
        if (Status != PaymentStatus.AwaitingPayment)
            throw new Exception("Não é possível associar uma nova sessão a um pagamento já processado.");

        StripeSessionId = sessionId;
        SetUpdatedAt();
    }

    internal void UpdateStripeSessionURL(string sessionUrl)
    {
        if (Status != PaymentStatus.AwaitingPayment)
            throw new Exception("Não é possível associar uma nova sessão a um pagamento já processado.");

        StripeSessionURL = sessionUrl;
        SetUpdatedAt();
    }
}
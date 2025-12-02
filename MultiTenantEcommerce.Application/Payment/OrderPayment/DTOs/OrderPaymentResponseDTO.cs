namespace MultiTenantEcommerce.Application.Payment.OrderPayment.DTOs;
public class OrderPaymentResponseDTO
{
    public Guid PaymentId { get; init; }
    public Guid CustomerId { get; init; }
    public Guid OrderId { get; init; }
    public decimal Total { get; init; }
    public string Status { get; init; }
    public string PaymentMethod { get; init; }
    public string? TransactionId { get; init; }
    public DateTime? CompletedAt { get; init; }
    public DateTime? CancelledAt { get; init; }
    public string? Metadata { get; init; }
}

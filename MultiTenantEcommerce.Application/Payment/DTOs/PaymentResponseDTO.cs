namespace MultiTenantEcommerce.Application.Payment.DTOs;
public class PaymentResponseDTO
{
    public Guid PaymentId { get; init; }
    public Guid PayerId { get; init; }
    public Guid PayeeId { get; init; }
    public string Reason { get; init; }
    public Guid ReasonId { get; init; }
    public decimal Amount { get; init; }
    public string Status { get; init; }
    public string PaymentMethod { get; init; }
    public string? TransactionId { get; init; }
    public DateTime? CompletedAt { get; init; }
    public DateTime? CancelledAt { get; init; }
    public string? Metadata { get; init; }
}

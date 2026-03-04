namespace MultiTenantEcommerce.Application.Commerce.Inventory.Common.DTOs;

public class StockMovementResponseDTO
{
    public Guid ProductId { get; init; }
    public int Quantity { get; init; }
    public string Reason { get; init; }
    public DateTime DateTime { get; init; }
}
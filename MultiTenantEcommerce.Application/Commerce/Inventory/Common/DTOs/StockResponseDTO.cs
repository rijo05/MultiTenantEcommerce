namespace MultiTenantEcommerce.Application.Commerce.Inventory.Common.DTOs;

public class StockResponseDTO : IStockDTO
{
    public int AvailableStock { get; init; }
}
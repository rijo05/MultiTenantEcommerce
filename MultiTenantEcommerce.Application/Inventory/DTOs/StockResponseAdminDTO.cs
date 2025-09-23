namespace MultiTenantEcommerce.Application.Inventory.DTOs;
public class StockResponseAdminDTO : IStockDTO
{
    public int Quantity { get; init; }
    public int MinimumQuantity { get; init; }
    public int Reserved { get; init; }
    public int AvailableStock { get; init; }
}

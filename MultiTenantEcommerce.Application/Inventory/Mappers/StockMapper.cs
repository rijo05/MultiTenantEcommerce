using MultiTenantEcommerce.Application.Inventory.DTOs;
using MultiTenantEcommerce.Domain.Inventory.Entities;

namespace MultiTenantEcommerce.Application.Inventory.Mappers;
public class StockMapper
{
    public StockResponseDTO ToStockResponseDTO(Stock stock)
    {
        return new StockResponseDTO()
        {
            AvailableStock = stock.StockAvailableAtMoment.Value,
        };
    }

    public StockResponseAdminDTO ToStockResponseAdminDTO(Stock stock)
    {
        return new StockResponseAdminDTO()
        {
            AvailableStock = stock.StockAvailableAtMoment.Value,
            MinimumQuantity = stock.MinimumQuantity.Value,
            Quantity = stock.Quantity.Value,
            Reserved = stock.Reserved.Value
        };
    }

    public StockMovementResponseDTO ToStockMovementResponseDTO(StockMovement movement)
    {
        return new StockMovementResponseDTO()
        {
            ProductId = movement.ProductId,
            Quantity = movement.Quantity,
            DateTime = movement.CreatedAt,
            Reason = movement.Reason.ToString(),
        };
    }

    public List<StockMovementResponseDTO> ToStockMovementResponseDTOList(IEnumerable<StockMovement> movements)
    {
        return movements.Select(x => ToStockMovementResponseDTO(x)).ToList();
    }
}

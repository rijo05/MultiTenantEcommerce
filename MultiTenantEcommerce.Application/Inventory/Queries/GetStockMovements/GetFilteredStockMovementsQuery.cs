using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Inventory.DTOs;
using MultiTenantEcommerce.Domain.Enums;

namespace MultiTenantEcommerce.Application.Inventory.Queries.GetStockMovements;
public record GetFilteredStockMovementsQuery(
    Guid? ProductId,
    int? MinQuantity,
    int? MaxQuantity,
    string? StockMovementReason,
    DateTime? MinDate,
    DateTime? MaxDate,
    int Page = 1,
    int PageSize = 20,
    SortOptions Sort = SortOptions.TimeDesc) : IQuery<List<StockMovementResponseDTO>>;

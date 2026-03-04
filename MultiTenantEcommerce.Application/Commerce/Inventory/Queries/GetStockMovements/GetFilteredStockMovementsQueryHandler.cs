using MultiTenantEcommerce.Application.Commerce.Inventory.Common.DTOs;
using MultiTenantEcommerce.Application.Commerce.Inventory.Common.Mappers;
using MultiTenantEcommerce.Domain.Commerce.Inventory.Interfaces;

namespace MultiTenantEcommerce.Application.Commerce.Inventory.Queries.GetStockMovements;

public class
    GetFilteredStockMovementsQueryHandler : IQueryHandler<GetFilteredStockMovementsQuery,
    List<StockMovementResponseDTO>>
{
    private readonly StockMapper _stockMapper;
    private readonly IStockMovementRepository _stockRepository;

    public GetFilteredStockMovementsQueryHandler(IStockMovementRepository stockRepository,
        StockMapper stockMapper)
    {
        _stockRepository = stockRepository;
        _stockMapper = stockMapper;
    }

    public async Task<List<StockMovementResponseDTO>> Handle(GetFilteredStockMovementsQuery request,
        CancellationToken cancellationToken)
    {
        var movements = await _stockRepository.GetFilteredAsync(request.ProductId,
            request.MinQuantity,
            request.MaxQuantity,
            request.StockMovementReason,
            request.MinDate,
            request.MaxDate,
            request.Page,
            request.PageSize,
            request.Sort);

        return _stockMapper.ToStockMovementResponseDTOList(movements);
    }
}
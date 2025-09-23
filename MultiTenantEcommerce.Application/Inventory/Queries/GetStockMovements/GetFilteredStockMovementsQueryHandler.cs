using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Inventory.DTOs;
using MultiTenantEcommerce.Application.Inventory.Mappers;
using MultiTenantEcommerce.Domain.Inventory.Interfaces;

namespace MultiTenantEcommerce.Application.Inventory.Queries.GetStockMovements;
public class GetFilteredStockMovementsQueryHandler : IQueryHandler<GetFilteredStockMovementsQuery, List<StockMovementResponseDTO>>
{
    private readonly IStockMovementRepository _stockRepository;
    private readonly StockMapper _stockMapper;

    public GetFilteredStockMovementsQueryHandler(IStockMovementRepository stockRepository,
        StockMapper stockMapper)
    {
        _stockRepository = stockRepository;
        _stockMapper = stockMapper;
    }

    public async Task<List<StockMovementResponseDTO>> Handle(GetFilteredStockMovementsQuery request, CancellationToken cancellationToken)
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

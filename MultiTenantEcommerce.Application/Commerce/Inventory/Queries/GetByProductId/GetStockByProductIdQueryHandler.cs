using MultiTenantEcommerce.Application.Commerce.Inventory.Common.DTOs;
using MultiTenantEcommerce.Application.Commerce.Inventory.Common.Mappers;
using MultiTenantEcommerce.Domain.Commerce.Inventory.Interfaces;

namespace MultiTenantEcommerce.Application.Commerce.Inventory.Queries.GetByProductId;

public class GetStockByProductIdQueryHandler : IQueryHandler<GetStockByProductIdQuery, IStockDTO>
{
    private readonly StockMapper _stockMapper;
    private readonly IStockRepository _stockRepository;

    public GetStockByProductIdQueryHandler(IStockRepository stockRepository, StockMapper stockMapper)
    {
        _stockRepository = stockRepository;
        _stockMapper = stockMapper;
    }

    public async Task<IStockDTO> Handle(GetStockByProductIdQuery request, CancellationToken cancellationToken)
    {
        var stock = await _stockRepository.GetByProductIdAsync(request.ProductId)
                    ?? throw new Exception("Stock couldnt be found");

        return request.IsAdmin
            ? _stockMapper.ToStockResponseAdminDTO(stock)
            : _stockMapper.ToStockResponseDTO(stock);
    }
}
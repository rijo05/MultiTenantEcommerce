using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Inventory.DTOs;
using MultiTenantEcommerce.Application.Inventory.Mappers;
using MultiTenantEcommerce.Domain.Inventory.Interfaces;

namespace MultiTenantEcommerce.Application.Inventory.Queries.GetByProductId;
public class GetStockByProductIdQueryHandler : IQueryHandler<GetStockByProductIdQuery, IStockDTO>
{
    private readonly IStockRepository _stockRepository;
    private readonly StockMapper _stockMapper;

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

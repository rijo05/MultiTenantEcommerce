using MultiTenantEcommerce.Application.Commerce.Inventory.Common.DTOs;
using MultiTenantEcommerce.Application.Commerce.Inventory.Common.Mappers;
using MultiTenantEcommerce.Domain.Commerce.Inventory.Interfaces;

namespace MultiTenantEcommerce.Application.Commerce.Inventory.Commands.IncrementStock;

public class IncrementStockCommandHandler : ICommandHandler<IncrementStockCommand, StockResponseAdminDTO>
{
    private readonly StockMapper _stockMapper;
    private readonly IStockRepository _stockRepository;
    private readonly IUnitOfWork _unitOfWork;

    public IncrementStockCommandHandler(IStockRepository stockRepository,
        StockMapper stockMapper,
        IUnitOfWork unitOfWork)
    {
        _stockRepository = stockRepository;
        _stockMapper = stockMapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<StockResponseAdminDTO> Handle(IncrementStockCommand request, CancellationToken cancellationToken)
    {
        var stock = await _stockRepository.GetByProductIdAsync(request.ProductId)
                    ?? throw new Exception("Stock couldnt be found");

        stock.IncrementStock(request.Quantity);

        await _unitOfWork.CommitAsync();

        return _stockMapper.ToStockResponseAdminDTO(stock);
    }
}
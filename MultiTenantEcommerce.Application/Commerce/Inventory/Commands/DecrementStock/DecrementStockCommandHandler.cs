using MultiTenantEcommerce.Application.Commerce.Inventory.Common.DTOs;
using MultiTenantEcommerce.Application.Commerce.Inventory.Common.Mappers;
using MultiTenantEcommerce.Domain.Commerce.Inventory.Enums;
using MultiTenantEcommerce.Domain.Commerce.Inventory.Interfaces;

namespace MultiTenantEcommerce.Application.Commerce.Inventory.Commands.DecrementStock;

public class DecrementStockCommandHandler : ICommandHandler<DecrementStockCommand, StockResponseAdminDTO>
{
    private readonly StockMapper _stockMapper;
    private readonly IStockRepository _stockRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DecrementStockCommandHandler(IStockRepository stockRepository,
        StockMapper stockMapper,
        IUnitOfWork unitOfWork)
    {
        _stockRepository = stockRepository;
        _stockMapper = stockMapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<StockResponseAdminDTO> Handle(DecrementStockCommand request, CancellationToken cancellationToken)
    {
        var stock = await _stockRepository.GetByProductIdAsync(request.ProductId)
                    ?? throw new Exception("Stock couldnt be found");

        stock.DecrementStock(request.Quantity, StockMovementReason.Adjustment);

        await _unitOfWork.CommitAsync();

        return _stockMapper.ToStockResponseAdminDTO(stock);
    }
}
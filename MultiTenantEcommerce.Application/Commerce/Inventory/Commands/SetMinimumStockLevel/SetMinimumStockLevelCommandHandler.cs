using MultiTenantEcommerce.Application.Commerce.Inventory.Common.DTOs;
using MultiTenantEcommerce.Application.Commerce.Inventory.Common.Mappers;
using MultiTenantEcommerce.Domain.Commerce.Inventory.Interfaces;

namespace MultiTenantEcommerce.Application.Commerce.Inventory.Commands.SetMinimumStockLevel;

public class SetMinimumStockLevelCommandHandler : ICommandHandler<SetMinimumStockLevelCommand, StockResponseAdminDTO>
{
    private readonly StockMapper _stockMapper;
    private readonly IStockRepository _stockRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SetMinimumStockLevelCommandHandler(IStockRepository stockRepository,
        StockMapper stockMapper,
        IUnitOfWork unitOfWork)
    {
        _stockRepository = stockRepository;
        _stockMapper = stockMapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<StockResponseAdminDTO> Handle(SetMinimumStockLevelCommand request,
        CancellationToken cancellationToken)
    {
        var stock = await _stockRepository.GetByProductIdAsync(request.ProductId)
                    ?? throw new Exception("Stock couldnt be found");

        stock.SetMinimumStockLevel(request.Quantity);

        await _unitOfWork.CommitAsync();

        return _stockMapper.ToStockResponseAdminDTO(stock);
    }
}
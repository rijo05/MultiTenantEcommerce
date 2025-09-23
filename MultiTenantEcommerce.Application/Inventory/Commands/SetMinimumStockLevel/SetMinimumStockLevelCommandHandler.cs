using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Common.Interfaces.Persistence;
using MultiTenantEcommerce.Application.Inventory.DTOs;
using MultiTenantEcommerce.Application.Inventory.Mappers;
using MultiTenantEcommerce.Domain.Inventory.Interfaces;

namespace MultiTenantEcommerce.Application.Inventory.Commands.SetMinimumStockLevel;
public class SetMinimumStockLevelCommandHandler : ICommandHandler<SetMinimumStockLevelCommand, StockResponseAdminDTO>
{
    private readonly IStockRepository _stockRepository;
    private readonly StockMapper _stockMapper;
    private readonly IUnitOfWork _unitOfWork;

    public SetMinimumStockLevelCommandHandler(IStockRepository stockRepository,
        StockMapper stockMapper,
        IUnitOfWork unitOfWork)
    {
        _stockRepository = stockRepository;
        _stockMapper = stockMapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<StockResponseAdminDTO> Handle(SetMinimumStockLevelCommand request, CancellationToken cancellationToken)
    {
        var stock = await _stockRepository.GetByProductIdAsync(request.ProductId)
            ?? throw new Exception("Stock couldnt be found");

        stock.SetMinimumStockLevel(request.Quantity);

        await _unitOfWork.CommitAsync();

        return _stockMapper.ToStockResponseAdminDTO(stock);
    }
}

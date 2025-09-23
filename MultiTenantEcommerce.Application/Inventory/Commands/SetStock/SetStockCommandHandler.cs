using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Common.Interfaces.Persistence;
using MultiTenantEcommerce.Application.Inventory.DTOs;
using MultiTenantEcommerce.Application.Inventory.Mappers;
using MultiTenantEcommerce.Domain.Inventory.Interfaces;

namespace MultiTenantEcommerce.Application.Inventory.Commands.SetStock;
public class SetStockCommandHandler : ICommandHandler<SetStockCommand, StockResponseAdminDTO>
{
    private readonly IStockRepository _stockRepository;
    private readonly StockMapper _stockMapper;
    private readonly IUnitOfWork _unitOfWork;

    public SetStockCommandHandler(IStockRepository stockRepository,
        StockMapper stockMapper,
        IUnitOfWork unitOfWork)
    {
        _stockRepository = stockRepository;
        _stockMapper = stockMapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<StockResponseAdminDTO> Handle(SetStockCommand request, CancellationToken cancellationToken)
    {
        var stock = await _stockRepository.GetByProductIdAsync(request.ProductId)
            ?? throw new Exception("Stock couldnt be found");

        stock.SetStock(request.Quantity);

        await _unitOfWork.CommitAsync();

        return _stockMapper.ToStockResponseAdminDTO(stock);
    }
}

using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Common.Interfaces.Persistence;
using MultiTenantEcommerce.Application.Common.Interfaces.Services;
using MultiTenantEcommerce.Application.Sales.ShoppingCart.DTOs;
using MultiTenantEcommerce.Application.Sales.ShoppingCart.Mappers;
using MultiTenantEcommerce.Domain.Catalog.Interfaces;
using MultiTenantEcommerce.Domain.Sales.ShoppingCart.Interfaces;
using MultiTenantEcommerce.Domain.ValueObjects;

namespace MultiTenantEcommerce.Application.Sales.ShoppingCart.Commands.AddItem;
public class AddCartItemCommandHandler : ICommandHandler<AddCartItemCommand, CartResponseDTO>
{
    private readonly ICartRepository _cartRepository;
    private readonly CartMapper _cartMapper;
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStockService _stockService;

    public AddCartItemCommandHandler(ICartRepository cartRepository,
        CartMapper cartMapper,
        IProductRepository productRepository,
        IUnitOfWork unitOfWork,
        IStockService stockService)
    {
        _cartRepository = cartRepository;
        _cartMapper = cartMapper;
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
        _stockService = stockService;
    }

    public async Task<CartResponseDTO> Handle(AddCartItemCommand request, CancellationToken cancellationToken)
    {
        var cart = await _cartRepository.GetByCustomerIdAsync(request.CustomerId)
            ?? throw new Exception("Cart doesnt exist");

        var product = await _productRepository.GetByIdAsync(request.ProductId)
            ?? throw new Exception("Product doesnt exist");

        if (!await _stockService.CheckAvailability(request.ProductId, new PositiveQuantity(request.quantity)))
            throw new Exception($"Not enough stock for {request.ProductId}");

        cart.AddItem(product, new PositiveQuantity(request.quantity));

        await _unitOfWork.CommitAsync();

        return _cartMapper.ToCartResponseDTO(cart);
    }
}

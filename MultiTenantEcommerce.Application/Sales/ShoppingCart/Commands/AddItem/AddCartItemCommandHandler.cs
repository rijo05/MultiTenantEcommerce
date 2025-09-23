using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Common.Interfaces.Persistence;
using MultiTenantEcommerce.Application.Common.Interfaces.Services;
using MultiTenantEcommerce.Application.Sales.ShoppingCart.DTOs;
using MultiTenantEcommerce.Application.Sales.ShoppingCart.Mappers;
using MultiTenantEcommerce.Domain.Catalog.Interfaces;
using MultiTenantEcommerce.Domain.Sales.ShoppingCart.Entities;
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
    private readonly ITenantContext _tenantContext;

    public AddCartItemCommandHandler(ICartRepository cartRepository,
        CartMapper cartMapper,
        IProductRepository productRepository,
        IUnitOfWork unitOfWork,
        IStockService stockService,
        ITenantContext tenantContext)
    {
        _cartRepository = cartRepository;
        _cartMapper = cartMapper;
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
        _stockService = stockService;
        _tenantContext = tenantContext;
    }

    public async Task<CartResponseDTO> Handle(AddCartItemCommand request, CancellationToken cancellationToken)
    {
        var cart = await _cartRepository.GetByCustomerIdAsync(request.CustomerId);

        if (cart == null)
        {
            cart = new Cart(_tenantContext.TenantId, request.CustomerId);
            await _cartRepository.AddAsync(cart);
        }

        var product = await _productRepository.GetByIdAsync(request.ProductId)
            ?? throw new Exception("Product doesnt exist");

        if (!await _stockService.CheckAvailability(request.ProductId, new PositiveQuantity(request.Quantity)))
            throw new Exception($"Not enough stock for {request.ProductId}");


        cart.AddItem(product, new PositiveQuantity(request.Quantity));

        await _unitOfWork.CommitAsync();

        return _cartMapper.ToCartResponseDTO(cart);
    }
}

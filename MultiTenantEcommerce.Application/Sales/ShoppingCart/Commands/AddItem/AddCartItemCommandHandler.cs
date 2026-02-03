using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Common.Interfaces.Persistence;
using MultiTenantEcommerce.Application.Common.Interfaces.Services;
using MultiTenantEcommerce.Application.Sales.ShoppingCart.DTOs;
using MultiTenantEcommerce.Application.Sales.ShoppingCart.Mappers;
using MultiTenantEcommerce.Domain.Catalog.Interfaces;
using MultiTenantEcommerce.Domain.Inventory.Interfaces;
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
    private readonly IFileStorageService _fileStorageService;
    private readonly IStockRepository _stockRepository;

    public AddCartItemCommandHandler(ICartRepository cartRepository,
        CartMapper cartMapper,
        IProductRepository productRepository,
        IUnitOfWork unitOfWork,
        IStockService stockService,
        ITenantContext tenantContext,
        IFileStorageService fileStorageService,
        IStockRepository stockRepository)
    {
        _cartRepository = cartRepository;
        _cartMapper = cartMapper;
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
        _stockService = stockService;
        _tenantContext = tenantContext;
        _fileStorageService = fileStorageService;
        _stockRepository = stockRepository;
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

        var productIds = cart.Items.Select(x => x.Id);
        var products = await _productRepository.GetByIdsAsync(productIds);
        products.Add(product);

        if (!await _stockService.CheckAvailability(request.ProductId, new PositiveQuantity(request.Quantity)))
            throw new Exception($"Not enough stock for {request.ProductId}");
        var stocks = await _stockRepository.GetBulkByProductIdsAsync(products.Select(x => x.Id));

        cart.AddItem(product.Id, new PositiveQuantity(request.Quantity));

        var images = _fileStorageService.GetPresignedUrls(products.SelectMany(x => x.Images).Select(x => x.Key).ToList());

        await _unitOfWork.CommitAsync();

        return _cartMapper.ToCartResponseDTO(cart, products, stocks, images);
    }
}

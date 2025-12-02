using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Common.Interfaces.Persistence;
using MultiTenantEcommerce.Application.Sales.ShoppingCart.DTOs;
using MultiTenantEcommerce.Application.Sales.ShoppingCart.Mappers;
using MultiTenantEcommerce.Domain.Catalog.Interfaces;
using MultiTenantEcommerce.Domain.Inventory.Interfaces;
using MultiTenantEcommerce.Domain.Sales.ShoppingCart.Interfaces;

namespace MultiTenantEcommerce.Application.Sales.ShoppingCart.Commands.RemoveItem;
public class RemoveCartItemCommandHandler : ICommandHandler<RemoveCartItemCommand, CartResponseDTO>
{
    private readonly ICartRepository _cartRepository;
    private readonly IProductRepository _productRepository;
    private readonly IStockRepository _stockRepository;
    private readonly IFileStorageService _fileStorageService;
    private readonly CartMapper _cartMapper;
    private readonly IUnitOfWork _unitOfWork;

    public RemoveCartItemCommandHandler(ICartRepository cartRepository, 
        IProductRepository productRepository, 
        IStockRepository stockRepository, 
        IFileStorageService fileStorageService, 
        CartMapper cartMapper, 
        IUnitOfWork unitOfWork)
    {
        _cartRepository = cartRepository;
        _productRepository = productRepository;
        _stockRepository = stockRepository;
        _fileStorageService = fileStorageService;
        _cartMapper = cartMapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<CartResponseDTO> Handle(RemoveCartItemCommand request, CancellationToken cancellationToken)
    {
        var cart = await _cartRepository.GetByCustomerIdAsync(request.CustomerId)
            ?? throw new Exception("Cart doesnt exist");

        cart.RemoveItem(request.ProductId);

        var productIds = cart.Items.Select(x => x.Id);
        var products = await _productRepository.GetByIdsAsync(productIds);
        var stocks = await _stockRepository.GetBulkByProductIdsAsync(productIds);
        var images = _fileStorageService.GetPresignedUrls(products.SelectMany(x => x.Images).Select(x => x.Key).ToList());

        await _unitOfWork.CommitAsync();

        return _cartMapper.ToCartResponseDTO(cart, products, stocks, images);
    }
}

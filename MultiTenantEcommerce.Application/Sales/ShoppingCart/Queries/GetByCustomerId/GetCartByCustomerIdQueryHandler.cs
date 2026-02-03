using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Common.Interfaces.Persistence;
using MultiTenantEcommerce.Application.Sales.ShoppingCart.DTOs;
using MultiTenantEcommerce.Application.Sales.ShoppingCart.Mappers;
using MultiTenantEcommerce.Domain.Catalog.Interfaces;
using MultiTenantEcommerce.Domain.Inventory.Interfaces;
using MultiTenantEcommerce.Domain.Sales.ShoppingCart.Interfaces;

namespace MultiTenantEcommerce.Application.Sales.ShoppingCart.Queries.GetByCustomerId;
public class GetCartByCustomerIdQueryHandler : IQueryHandler<GetCartByCustomerIdQuery, CartResponseDTO>
{
    private readonly ICartRepository _cartRepository;
    private readonly IProductRepository _productRepository;
    private readonly IStockRepository _stockRepository;
    private readonly IFileStorageService _fileStorageService;
    private readonly CartMapper _cartMapper;

    public GetCartByCustomerIdQueryHandler(ICartRepository cartRepository,
        IProductRepository productRepository,
        IStockRepository stockRepository,
        IFileStorageService fileStorageService,
        CartMapper cartMapper)
    {
        _cartRepository = cartRepository;
        _productRepository = productRepository;
        _stockRepository = stockRepository;
        _fileStorageService = fileStorageService;
        _cartMapper = cartMapper;
    }

    public async Task<CartResponseDTO> Handle(GetCartByCustomerIdQuery request, CancellationToken cancellationToken)
    {
        var cart = await _cartRepository.GetByCustomerIdAsync(request.CustomerId)
            ?? throw new Exception("Customer doesnt have an open cart");

        var productIds = cart.Items.Select(x => x.Id);
        var products = await _productRepository.GetByIdsAsync(productIds);
        var stocks = await _stockRepository.GetBulkByProductIdsAsync(productIds);
        var images = _fileStorageService.GetPresignedUrls(products.SelectMany(x => x.Images).Select(x => x.Key).ToList());

        return _cartMapper.ToCartResponseDTO(cart, products, stocks, images);
    }
}

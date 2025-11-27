using MultiTenantEcommerce.Application.Catalog.Products.DTOs.Products;
using MultiTenantEcommerce.Application.Catalog.Products.Mappers;
using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Common.Interfaces.Persistence;
using MultiTenantEcommerce.Domain.Catalog.Interfaces;
using MultiTenantEcommerce.Domain.Inventory.Interfaces;

namespace MultiTenantEcommerce.Application.Catalog.Products.Queries.GetById;
public class GetProductByIdQueryHandler : IQueryHandler<GetProductByIdQuery, IProductDTO>
{
    private readonly IProductRepository _productRepository;
    private readonly ProductMapper _productMapper;
    private readonly IStockRepository _stockRepository;
    private readonly IFileStorageService _fileStorageService;

    public GetProductByIdQueryHandler(IProductRepository productRepository,
        ProductMapper productMapper,
        IStockRepository stockRepository,
        IFileStorageService fileStorageService)
    {
        _productRepository = productRepository;
        _productMapper = productMapper;
        _stockRepository = stockRepository;
        _fileStorageService = fileStorageService;
    }

    public async Task<IProductDTO> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdIncluding(request.ProductId, x => x.Category, x => x.Images)
            ?? throw new Exception("Product not found");

        var stock = await _stockRepository.GetByProductIdAsync(product.Id)
            ?? throw new Exception("Stock not found. This shouldnt happen");

        var images = _fileStorageService.GetImageUrl(product.Images.Select(x => x.Key).ToList());

        return request.IsAdmin
            ? _productMapper.ToProductResponseAdminDTO(product, stock, images)
            : _productMapper.ToProductResponseDTO(product, stock, images);
    }
}

using MultiTenantEcommerce.Application.Catalog.Products.DTOs.Products;
using MultiTenantEcommerce.Application.Catalog.Products.Mappers;
using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Common.Interfaces.Persistence;
using MultiTenantEcommerce.Domain.Catalog.Interfaces;
using MultiTenantEcommerce.Domain.Inventory.Interfaces;

namespace MultiTenantEcommerce.Application.Catalog.Products.Commands.SetMainImage;
public class SetMainProductImageCommandHandler : ICommandHandler<SetMainProductImageCommand, ProductResponseAdminDTO>
{
    private readonly IProductRepository _productRepository;
    private readonly ProductMapper _productMapper;
    private readonly IFileStorageService _fileStorageService;
    private readonly IStockRepository _stockRepository;

    public SetMainProductImageCommandHandler(IProductRepository productRepository,
        ProductMapper productMapper,
        IFileStorageService fileStorageService,
        IStockRepository stockRepository)
    {
        _productRepository = productRepository;
        _productMapper = productMapper;
        _fileStorageService = fileStorageService;
        _stockRepository = stockRepository;
    }

    public async Task<ProductResponseAdminDTO> Handle(SetMainProductImageCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId)
            ?? throw new Exception("Product doesnt exist");

        var image = product.Images.FirstOrDefault(x => x.Id == request.ImageId)
            ?? throw new Exception("Image doesnt exist");

        var stock = await _stockRepository.GetByProductIdAsync(product.Id)
            ?? throw new Exception("Stock not found. This shouldnt happen");

        var images = _fileStorageService.GetPresignedUrls(product.Images.Select(x => x.Key).ToList());

        product.MarkAsMain(request.ImageId);

        return _productMapper.ToProductResponseAdminDTO(product, stock, images);
    }
}

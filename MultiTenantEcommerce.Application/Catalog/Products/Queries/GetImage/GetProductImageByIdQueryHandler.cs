using MultiTenantEcommerce.Application.Catalog.Products.DTOs.Images;
using MultiTenantEcommerce.Application.Catalog.Products.Mappers;
using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Common.Interfaces.Persistence;
using MultiTenantEcommerce.Domain.Catalog.Interfaces;

namespace MultiTenantEcommerce.Application.Catalog.Products.Queries.GetImage;
public class GetProductImageByIdQueryHandler : IQueryHandler<GetProductImageByIdQuery, IProductImageDTO>
{
    private readonly IFileStorageService _fileStorageService;
    private readonly IProductRepository _productRepository;
    private readonly ImageMapper _imageMapper;

    public GetProductImageByIdQueryHandler(IFileStorageService fileStorageService,
        IProductRepository productRepository,
        ImageMapper imageMapper)
    {
        _fileStorageService = fileStorageService;
        _productRepository = productRepository;
        _imageMapper = imageMapper;
    }

    public async Task<IProductImageDTO> Handle(GetProductImageByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId)
            ?? throw new Exception("Product doesnt exist");

        var targetImage = product.Images.FirstOrDefault(x => x.Id == request.ImageId)
            ?? throw new Exception("Image not found for this product");

        var signedUrls = _fileStorageService.GetPresignedUrls(new List<string> { targetImage.Key });

        var singleImageList = new[] { targetImage };

        return request.IsAdmin
            ? _imageMapper.ToProductImageResponseAdminDTO(targetImage, signedUrls)
            : _imageMapper.ToProductImageResponseDTO(targetImage, signedUrls);
    }
}

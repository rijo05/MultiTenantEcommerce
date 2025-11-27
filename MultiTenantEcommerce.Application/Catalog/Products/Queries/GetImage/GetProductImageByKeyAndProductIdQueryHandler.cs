using MultiTenantEcommerce.Application.Catalog.Products.DTOs.Images;
using MultiTenantEcommerce.Application.Catalog.Products.Mappers;
using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Common.Interfaces.Persistence;
using MultiTenantEcommerce.Domain.Catalog.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Application.Catalog.Products.Queries.GetImage;
public class GetProductImageByKeyAndProductIdQueryHandler : IQueryHandler<GetProductImageByKeyAndProductIdQuery, IProductImageDTO>
{
    private readonly IFileStorageService _fileStorageService;
    private readonly IProductRepository _productRepository;
    private readonly ImageMapper _imageMapper;

    public GetProductImageByKeyAndProductIdQueryHandler(IFileStorageService fileStorageService, 
        IProductRepository productRepository, 
        ImageMapper imageMapper)
    {
        _fileStorageService = fileStorageService;
        _productRepository = productRepository;
        _imageMapper = imageMapper;
    }

    public async Task<IProductImageDTO> Handle(GetProductImageByKeyAndProductIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdIncluding(request.ProductId, x => x.Images) 
            ?? throw new Exception("Product doesnt exist");

        var targetImage = product.Images.FirstOrDefault(x => x.Key == request.Key)
            ?? throw new Exception("Image not found for this product");

        var signedUrls = _fileStorageService.GetImageUrl(new List<string> { request.Key });

        var dto = request.IsAdmin 
            ? _imageMapper.ToProductImageResponseAdminDTO(product.Images.Where(x => x.Key == request.Key), signedUrls)
            : _imageMapper.ToProductImageResponseDTO(product.Images.Where(x => x.Key == request.Key), signedUrls);

        //GetProductImageByKeyAndProductIdQuery returns ONE image
        return dto[0];
    }
}

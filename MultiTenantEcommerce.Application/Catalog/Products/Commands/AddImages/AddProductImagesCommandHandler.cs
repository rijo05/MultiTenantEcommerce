using MultiTenantEcommerce.Application.Common.DTOs;
using MultiTenantEcommerce.Application.Common.Helpers;
using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Common.Interfaces.Persistence;
using MultiTenantEcommerce.Domain.Catalog.Entities;
using MultiTenantEcommerce.Domain.Catalog.Interfaces;
using MultiTenantEcommerce.Domain.Tenants.Interfaces;

namespace MultiTenantEcommerce.Application.Catalog.Products.Commands.AddImages;
public class AddProductImagesCommandHandler : ICommandHandler<AddProductImagesCommand, List<PresignedUpload>>
{
    private readonly IFileStorageService _fileStorageService;
    private readonly IProductRepository _productRepository;
    private readonly ITenantRepository _tenantRepository;
    private readonly ITenantContext _tenantContext;
    private readonly IUnitOfWork _unitOfWork;

    public AddProductImagesCommandHandler(IFileStorageService fileStorageService,
        IProductRepository productRepository,
        ITenantRepository tenantRepository,
        ITenantContext tenantContext,
        IUnitOfWork unitOfWork)
    {
        _fileStorageService = fileStorageService;
        _productRepository = productRepository;
        _tenantRepository = tenantRepository;
        _tenantContext = tenantContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<List<PresignedUpload>> Handle(AddProductImagesCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdIncluding(request.ProductId, x => x.Images)
            ?? throw new Exception("Product not found");

        var tenant = await _tenantRepository.GetByIdAsync(_tenantContext.TenantId)
            ?? throw new Exception("tenant doesnt exist (shouldnt happen)");

        //criar planos #########
        var maximage = 5;
        var maxsize = 1000;
        var listContentType = new List<string> { MimeTypes.Png, MimeTypes.Jpeg, MimeTypes.Jpg };

        if (product.Images.Count >= maximage)
            throw new Exception("This product cant have more images");

        var images = new List<ProductImages>();

        foreach (var img in request.Metadata)
        {
            if (img.Size > maxsize)
                throw new Exception("File too big");

            if (!listContentType.Contains(img.ContentType))
                throw new Exception("File not accepted");

            images.Add(product.AddImage(img.FileName, img.Size, img.ContentType, img.IsMain));
        }

        var uploads = _fileStorageService.GenerateUploadUrls(_tenantContext.TenantId, product.Id, images);

        await _unitOfWork.CommitAsync();

        return uploads;
    }
}

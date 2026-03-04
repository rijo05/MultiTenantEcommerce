using MultiTenantEcommerce.Domain.Commerce.Catalog.Entities;
using MultiTenantEcommerce.Domain.Commerce.Catalog.Interfaces;
using MultiTenantEcommerce.Shared.Application.CQRS;
using MultiTenantEcommerce.Shared.Infrastructure.Persistence;
using MultiTenantEcommerce.Shared.Infrastructure.Services;
using MultiTenantEcommerce.Shared.Integration.DTOs;
using MultiTenantEcommerce.Shared.Utilities.Constants;

namespace MultiTenantEcommerce.Application.Commerce.Catalog.Products.Commands.ManageImages;

public class AddProductImagesCommandHandler : ICommandHandler<AddProductImagesCommand, List<PresignedUpload>>
{
    private readonly IFileStorageService _fileStorageService;
    private readonly IProductRepository _productRepository;
    private readonly ITenantContext _tenantContext;
    private readonly IUnitOfWork _unitOfWork;

    public AddProductImagesCommandHandler(IFileStorageService fileStorageService,
        IProductRepository productRepository,
        ITenantContext tenantContext,
        IUnitOfWork unitOfWork)
    {
        _fileStorageService = fileStorageService;
        _productRepository = productRepository;
        _tenantContext = tenantContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<List<PresignedUpload>> Handle(AddProductImagesCommand request,
        CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId)
                      ?? throw new Exception("Product not found");

        var images = product.AddImages(request.Metadata);

        var uploads = _fileStorageService.GenerateUploadUrls(_tenantContext.TenantId, product.Id, images.Select(x => (x.Key, x.ContentType)).ToList());

        await _unitOfWork.CommitAsync();

        return uploads;
    }
}
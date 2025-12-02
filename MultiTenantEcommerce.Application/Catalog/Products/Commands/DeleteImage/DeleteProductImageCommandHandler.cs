using MediatR;
using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Common.Interfaces.Persistence;
using MultiTenantEcommerce.Domain.Catalog.Interfaces;

namespace MultiTenantEcommerce.Application.Catalog.Products.Commands.DeleteImage;
public class DeleteProductImageCommandHandler : ICommandHandler<DeleteProductImageCommand, Unit>
{
    private readonly IProductRepository _productRepository;
    private readonly IFileStorageService _fileStorageService;

    public DeleteProductImageCommandHandler(IProductRepository productRepository, IFileStorageService fileStorageService)
    {
        _productRepository = productRepository;
        _fileStorageService = fileStorageService;
    }

    public async Task<Unit> Handle(DeleteProductImageCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId)
            ?? throw new Exception("Product doesnt exist");

        var image = product.Images.FirstOrDefault(x => x.Id == request.ImageId)
            ?? throw new Exception("Image doesnt exist");

        product.DeleteImage(request.ImageId);

        await _fileStorageService.DeleteImageUrl(image.Key);

        return Unit.Value;
    }
}

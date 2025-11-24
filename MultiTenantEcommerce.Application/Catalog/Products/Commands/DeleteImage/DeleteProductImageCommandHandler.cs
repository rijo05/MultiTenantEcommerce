using MediatR;
using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Common.Interfaces.Persistence;
using MultiTenantEcommerce.Domain.Catalog.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        var product = await _productRepository.GetByIdIncluding(request.ProductId, x => x.Images)
            ?? throw new Exception("Product doesnt exist");

        if (!product.Images.Select(x => x.Key).Contains(request.Key))
            throw new Exception("Image doesnt exist");

        product.DeleteImage(request.Key);

        await _fileStorageService.DeleteImageUrl(request.Key);

        return Unit.Value;
    }
}

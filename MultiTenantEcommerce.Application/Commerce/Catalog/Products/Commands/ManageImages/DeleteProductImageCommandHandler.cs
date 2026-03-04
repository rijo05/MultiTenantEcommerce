using MediatR;
using MultiTenantEcommerce.Domain.Commerce.Catalog.Interfaces;
using MultiTenantEcommerce.Shared.Application.CQRS;
using MultiTenantEcommerce.Shared.Infrastructure.Persistence;
using MultiTenantEcommerce.Shared.Infrastructure.Services;

namespace MultiTenantEcommerce.Application.Commerce.Catalog.Products.Commands.ManageImages;

public class DeleteProductImageCommandHandler : ICommandHandler<DeleteProductImageCommand, Unit>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteProductImageCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(DeleteProductImageCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId)
                      ?? throw new Exception("Product doesnt exist");

        product.DeleteImage(request.ImageId);

        await _unitOfWork.CommitAsync();

        return Unit.Value;
    }
}
using MediatR;
using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Common.Interfaces.Persistence;
using MultiTenantEcommerce.Domain.Catalog.Interfaces;

namespace MultiTenantEcommerce.Application.Catalog.Products.Commands.Delete;
public class DeleteProductCommandHandler : ICommandHandler<DeleteProductCommand, Unit>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId);
        if (product == null)
            throw new Exception("Product not found");

        await _productRepository.DeleteAsync(product);

        await _unitOfWork.CommitAsync();

        return Unit.Value;
    }
}

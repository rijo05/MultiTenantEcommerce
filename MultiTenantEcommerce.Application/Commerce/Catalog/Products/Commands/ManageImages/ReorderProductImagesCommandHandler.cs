using MediatR;
using MultiTenantEcommerce.Domain.Commerce.Catalog.Interfaces;
using MultiTenantEcommerce.Shared.Application.CQRS;
using MultiTenantEcommerce.Shared.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Application.Commerce.Catalog.Products.Commands.ManageImages;
public class ReorderProductImagesCommandHandler : ICommandHandler<ReorderProductImagesCommand, Unit>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ReorderProductImagesCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(ReorderProductImagesCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId)
            ?? throw new Exception("Product not found");

        product.ReorderImages(request.OrderedImageIds);

        await _unitOfWork.CommitAsync();

        return Unit.Value;
    }
}

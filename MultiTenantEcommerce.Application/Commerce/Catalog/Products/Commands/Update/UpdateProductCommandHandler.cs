using MediatR;
using MultiTenantEcommerce.Application.Commerce.Catalog.Products.Common.DTOs.Products;
using MultiTenantEcommerce.Application.Commerce.Catalog.Products.Common.Mappers;
using MultiTenantEcommerce.Domain.Commerce.Catalog.Entities;
using MultiTenantEcommerce.Domain.Commerce.Catalog.Interfaces;
using MultiTenantEcommerce.Domain.Logistics.Inventory.Interfaces;
using MultiTenantEcommerce.Shared.Application.CQRS;
using MultiTenantEcommerce.Shared.Infrastructure.Persistence;
using MultiTenantEcommerce.Shared.Infrastructure.Services;

namespace MultiTenantEcommerce.Application.Commerce.Catalog.Products.Commands.Update;

public class UpdateProductCommandHandler : ICommandHandler<UpdateProductCommand, Unit>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProductCommandHandler(ICategoryRepository categoryRepository,
        IProductRepository productRepository,
        IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId)
                      ?? throw new Exception("Product doesnt exist.");

        if (request.CategoryId.HasValue && request.CategoryId.Value != product.CategoryId)
            if (!await _categoryRepository.ExistsAsync(request.CategoryId.Value))
                throw new Exception("Category doesnt exist");

        product.UpdateProduct(request.Name,
            request.Description,
            request.Price,
            request.IsActive,
            request.CategoryId);

        await _unitOfWork.CommitAsync();
        return Unit.Value;
    }
}
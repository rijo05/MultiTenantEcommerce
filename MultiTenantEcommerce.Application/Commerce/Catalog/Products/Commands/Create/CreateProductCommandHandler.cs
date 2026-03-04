using MultiTenantEcommerce.Application.Commerce.Catalog.Products.Common.DTOs.Products;
using MultiTenantEcommerce.Application.Commerce.Catalog.Products.Common.Mappers;
using MultiTenantEcommerce.Domain.Commerce.Catalog.Entities;
using MultiTenantEcommerce.Domain.Commerce.Catalog.Interfaces;
using MultiTenantEcommerce.Shared.Application.CQRS;
using MultiTenantEcommerce.Shared.Domain.ValueObjects;
using MultiTenantEcommerce.Shared.Infrastructure.Persistence;
using MultiTenantEcommerce.Shared.Infrastructure.Services;
using MultiTenantEcommerce.Shared.Integration.DTOs;
using MultiTenantEcommerce.Shared.Integration.Proxies;

namespace MultiTenantEcommerce.Application.Commerce.Catalog.Products.Commands.Create;

public class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, ProductResponseAdminDTO>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IProductRepository _productRepository;
    private readonly ITenantContext _tenantContext;
    private readonly IUnitOfWork _unitOfWork;

    public CreateProductCommandHandler(ICategoryRepository categoryRepository,
        IProductRepository productRepository,
        ITenantContext tenantContext,
        IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _productRepository = productRepository;
        _tenantContext = tenantContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<ProductResponseAdminDTO> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(request.CategoryId)
                       ?? throw new Exception("Category doesnt exist");

        var product = new Product(
            _tenantContext.TenantId,
            request.Name,
            new Money(request.Price),
            category,
            request.Description,
            request.IsActive);

        await _productRepository.AddAsync(product);

        var stock = new StockProxyDTO(product.Id, 0);

        await _unitOfWork.CommitAsync();
        return product.ToDTOAdmin(stock);
    }
}
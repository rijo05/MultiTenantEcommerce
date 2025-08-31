using MultiTenantEcommerce.Application.Catalog.Products.DTOs;
using MultiTenantEcommerce.Application.Catalog.Products.Mappers;
using MultiTenantEcommerce.Application.Common.Interfaces;
using MultiTenantEcommerce.Domain.Catalog.Entities;
using MultiTenantEcommerce.Domain.Catalog.Interfaces;
using MultiTenantEcommerce.Domain.Common.Interfaces;
using MultiTenantEcommerce.Domain.Inventory.Entities;
using MultiTenantEcommerce.Domain.Inventory.Interfaces;
using MultiTenantEcommerce.Domain.ValueObjects;
using MultiTenantEcommerce.Infrastructure.Persistence.Context;

namespace MultiTenantEcommerce.Application.Catalog.Products.Commands.Create;
public class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, ProductResponseDTO>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IProductRepository _productRepository;
    private readonly IStockRepository _stockRepository;
    private readonly TenantContext _tenantContext;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ProductMapper _productMapper;

    public CreateProductCommandHandler(ICategoryRepository categoryRepository, 
        IProductRepository productRepository, 
        IStockRepository stockRepository,
        ProductMapper productMapper,
        TenantContext tenantContext,
        IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _productRepository = productRepository;
        _stockRepository = stockRepository;
        _productMapper = productMapper;
        _tenantContext = tenantContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<ProductResponseDTO> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        if (await _categoryRepository.GetByIdAsync(request.CategoryId) is null)
            throw new Exception($"Category with ID {request.CategoryId} does not exist.");

        var product = new Product(
            _tenantContext.TenantId,
            request.Name,
            new Money(request.Price),
            request.CategoryId,
            request.Description
            );

        var stock = new Stock(
            product,
            _tenantContext.TenantId,
            request.Quantity,
            request.MinimumQuantity
            );

        await _productRepository.AddAsync(product);
        await _stockRepository.AddAsync(stock);

        await _unitOfWork.CommitAsync();
        return _productMapper.ToProductResponseDTO(product);
    }
}

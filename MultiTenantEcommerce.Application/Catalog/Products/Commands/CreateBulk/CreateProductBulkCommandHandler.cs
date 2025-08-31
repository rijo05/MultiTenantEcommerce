using MultiTenantEcommerce.Application.Catalog.Commands.Product.Create;
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

namespace MultiTenantEcommerce.Application.Catalog.Products.Commands.CreateBulk;
public class CreateProductBulkCommandHandler : ICommandHandler<CreateProductBulkCommand, List<ProductResponseDTO>>
{
    private readonly IProductRepository _productRepository;
    private readonly TenantContext _tenantContext;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ProductMapper _productMapper;

    public CreateProductBulkCommandHandler(
        IProductRepository productRepository, 
        ProductMapper productMapper,
        TenantContext tenantContext,
        IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _productMapper = productMapper;
        _tenantContext = tenantContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<List<ProductResponseDTO>> Handle(CreateProductBulkCommand request, CancellationToken cancellationToken)
    {
        var products = new List<Product>();

        foreach (var cmd in request.Products)
        {
            var product = new Product(
                _tenantContext.TenantId,
                cmd.Name,
                new Money(cmd.Price),
                cmd.CategoryId,
                cmd.Description
            );

            products.Add( product );
        }

        await _productRepository.AddBulkAsync(products);
        await _unitOfWork.CommitAsync();

        return _productMapper.ToProductResponseDTOList(products);
    }
}

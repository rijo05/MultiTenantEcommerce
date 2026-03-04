using MultiTenantEcommerce.Application.Commerce.Catalog.Products.Common.DTOs.Products;
using MultiTenantEcommerce.Application.Commerce.Catalog.Products.Common.Mappers;
using MultiTenantEcommerce.Domain.Commerce.Catalog.Interfaces;
using MultiTenantEcommerce.Shared.Application.CQRS;
using MultiTenantEcommerce.Shared.Infrastructure.Services;

namespace MultiTenantEcommerce.Application.Commerce.Catalog.Products.Queries.GetById;

public class GetProductByIdQueryHandler : IQueryHandler<GetProductByIdQuery, ProductResponseDTO>
{
    private readonly IProductRepository _productRepository;

    public GetProductByIdQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ProductResponseDTO> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId);

        if (product == null || !product.IsActive)
            throw new Exception("Product doesnt exist");

        return product.ToDTO();
    }
}
using MultiTenantEcommerce.Application.Catalog.Products.DTOs;
using MultiTenantEcommerce.Application.Catalog.Products.Mappers;
using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Domain.Catalog.Interfaces;

namespace MultiTenantEcommerce.Application.Catalog.Products.Queries.GetById;
public class GetProductByIdQueryHandler : IQueryHandler<GetProductByIdQuery, ProductResponseDTO>
{
    private readonly IProductRepository _productRepository;
    private readonly ProductMapper _productMapper;
    public GetProductByIdQueryHandler(IProductRepository productRepository,
        ProductMapper productMapper)
    {
        _productRepository = productRepository;
        _productMapper = productMapper;
    }

    public async Task<ProductResponseDTO> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdWithCategoryAsync(request.ProductId)
            ?? throw new Exception("Product not found");

        return _productMapper.ToProductResponseDTO(product);
    }
}

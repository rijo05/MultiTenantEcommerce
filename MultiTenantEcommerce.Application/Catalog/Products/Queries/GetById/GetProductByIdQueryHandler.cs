using MultiTenantEcommerce.Application.Catalog.Products.DTOs;
using MultiTenantEcommerce.Application.Catalog.Products.Mappers;
using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Domain.Catalog.Interfaces;
using MultiTenantEcommerce.Domain.Inventory.Interfaces;

namespace MultiTenantEcommerce.Application.Catalog.Products.Queries.GetById;
public class GetProductByIdQueryHandler : IQueryHandler<GetProductByIdQuery, IProductDTO>
{
    private readonly IProductRepository _productRepository;
    private readonly ProductMapper _productMapper;
    private readonly IStockRepository _stockRepository;

    public GetProductByIdQueryHandler(IProductRepository productRepository,
        ProductMapper productMapper,
        IStockRepository stockRepository)
    {
        _productRepository = productRepository;
        _productMapper = productMapper;
        _stockRepository = stockRepository;
    }

    public async Task<IProductDTO> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdWithCategoryAsync(request.ProductId)
            ?? throw new Exception("Product not found");

        var stock = await _stockRepository.GetByProductIdAsync(product.Id)
            ?? throw new Exception("Stock not found. This shouldnt happen");

        return request.IsAdmin
            ? _productMapper.ToProductResponseAdminDTO(product, stock)
            : _productMapper.ToProductResponseDTO(product, stock);
    }
}

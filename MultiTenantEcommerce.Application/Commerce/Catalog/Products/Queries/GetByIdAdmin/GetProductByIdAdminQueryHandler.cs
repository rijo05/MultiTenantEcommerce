using MultiTenantEcommerce.Application.Commerce.Catalog.Products.Common.DTOs.Products;
using MultiTenantEcommerce.Application.Commerce.Catalog.Products.Common.Mappers;
using MultiTenantEcommerce.Domain.Commerce.Catalog.Interfaces;
using MultiTenantEcommerce.Shared.Application.CQRS;
using MultiTenantEcommerce.Shared.Infrastructure.Services;
using MultiTenantEcommerce.Shared.Integration.Proxies;
using MultiTenantEcommerce.Shared.Integration.DTOs;

namespace MultiTenantEcommerce.Application.Commerce.Catalog.Products.Queries.GetByIdAdmin;
public class GetProductByIdAdminQueryHandler : IQueryHandler<GetProductByIdAdminQuery, ProductResponseAdminDTO>
{
    private readonly IProductRepository _productRepository;
    private readonly ILogisticsIntegrationProxy _stockIntegrationProxy;

    public GetProductByIdAdminQueryHandler(IProductRepository productRepository, 
        ILogisticsIntegrationProxy stockIntegrationProxy)
    {
        _productRepository = productRepository;
        _stockIntegrationProxy = stockIntegrationProxy;
    }

    public async Task<ProductResponseAdminDTO> Handle(GetProductByIdAdminQuery request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId)
                      ?? throw new Exception("Product not found");

        var stock = await _stockIntegrationProxy.GetStocksByProductIdsAsync(new List<Guid>() { product.Id });
        var productStock = stock.FirstOrDefault() ?? new StockProxyDTO(product.Id, 0);

        return product.ToDTOAdmin(productStock);
    }
}

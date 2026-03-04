using MultiTenantEcommerce.Application.Commerce.Catalog.Products.Common.DTOs.Products;
using MultiTenantEcommerce.Application.Commerce.Catalog.Products.Common.Mappers;
using MultiTenantEcommerce.Domain.Commerce.Catalog.Interfaces;
using MultiTenantEcommerce.Shared.Application;
using MultiTenantEcommerce.Shared.Application.CQRS;
using MultiTenantEcommerce.Shared.Infrastructure.Services;
using MultiTenantEcommerce.Shared.Integration.Proxies;

namespace MultiTenantEcommerce.Application.Commerce.Catalog.Products.Queries.GetFilteredAdmin;
public class GetFilteredProductsAdminQueryHandler : IQueryHandler<GetFilteredProductsAdminQuery, PaginatedList<ProductResponseAdminDTO>>
{
    private readonly IProductRepository _productRepository;
    private readonly ILogisticsIntegrationProxy _stockIntegrationProxy;

    public GetFilteredProductsAdminQueryHandler(IProductRepository productRepository, 
        ILogisticsIntegrationProxy stockIntegrationProxy)
    {
        _productRepository = productRepository;
        _stockIntegrationProxy = stockIntegrationProxy;
    }

    public async Task<PaginatedList<ProductResponseAdminDTO>> Handle(GetFilteredProductsAdminQuery request, CancellationToken cancellationToken)
    {
        var products = await _productRepository.GetFilteredAsync(
            request.CategoryId,
            request.Name,
            request.MinPrice,
            request.MaxPrice,
            request.IsActive,
            request.StockStatuses,
            request.Page,
            request.PageSize,
            request.Sort);

        if (products.Items.Count == 0)
            return new PaginatedList<ProductResponseAdminDTO>(new List<ProductResponseAdminDTO>(), 0, request.Page, request.PageSize);

        var stocks = await _stockIntegrationProxy.GetStocksByProductIdsAsync(products.Items.Select(x => x.Id));

        return products.ToPaginatedDTOAdmin(stocks);
    }
}

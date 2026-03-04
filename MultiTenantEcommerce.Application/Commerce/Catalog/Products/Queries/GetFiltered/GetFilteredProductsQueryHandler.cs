using MultiTenantEcommerce.Application.Commerce.Catalog.Products.Common.DTOs.Products;
using MultiTenantEcommerce.Application.Commerce.Catalog.Products.Common.Mappers;
using MultiTenantEcommerce.Domain.Commerce.Catalog.Enums;
using MultiTenantEcommerce.Domain.Commerce.Catalog.Interfaces;
using MultiTenantEcommerce.Shared.Application;
using MultiTenantEcommerce.Shared.Application.CQRS;
using MultiTenantEcommerce.Shared.Infrastructure.Services;

namespace MultiTenantEcommerce.Application.Commerce.Catalog.Products.Queries.GetFiltered;

public class GetFilteredProductsQueryHandler : IQueryHandler<GetFilteredProductsQuery, PaginatedList<ProductResponseDTO>>
{
    private readonly IProductRepository _productRepository;

    public GetFilteredProductsQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<PaginatedList<ProductResponseDTO>> Handle(GetFilteredProductsQuery request, CancellationToken cancellationToken)
    {
        var stockFilter = request.OnlyAvailable == true
            ? new List<StockStatus> { StockStatus.InStock, StockStatus.LowStock }
            : null;

        var products = await _productRepository.GetFilteredAsync(
            request.CategoryId,
            request.Name,
            request.MinPrice,
            request.MaxPrice,
            true,
            stockFilter,
            request.Page,
            request.PageSize,
            request.Sort);

        if(products.Items.Count == 0)
            return new PaginatedList<ProductResponseDTO>(new List<ProductResponseDTO>(), 0, request.Page, request.PageSize);

        return products.ToPaginatedDTO();
    }
}
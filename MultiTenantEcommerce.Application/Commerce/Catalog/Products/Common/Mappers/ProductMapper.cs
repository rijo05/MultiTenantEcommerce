using MultiTenantEcommerce.Application.Commerce.Catalog.Categories.Common.Mappers;
using MultiTenantEcommerce.Application.Commerce.Catalog.Products.Common.DTOs.Products;
using MultiTenantEcommerce.Domain.Commerce.Catalog.Entities;
using MultiTenantEcommerce.Domain.Commerce.Catalog.Enums;
using MultiTenantEcommerce.Shared.Application;
using MultiTenantEcommerce.Shared.Integration.DTOs;

namespace MultiTenantEcommerce.Application.Commerce.Catalog.Products.Common.Mappers;

public static class ProductMapper
{
    public static ProductResponseDTO ToDTO(this Product product)
    {
        return new ProductResponseDTO(
            product.Id,
            product.Name,
            product.Description,
            product.Price.Value,
            product.CategoryId,
            product.StockStatus,
            product.Images.ToDTOList()
        );
    }

    public static List<ProductResponseDTO> ToDTOList(this IEnumerable<Product> products)
    {
        return products.Select(p => p.ToDTO()).ToList();
    }

    public static ProductResponseAdminDTO ToDTOAdmin(this Product product, StockProxyDTO stock)
    {
        var baseDto = product.ToDTO();

        return new ProductResponseAdminDTO(
            baseDto.Id,
            baseDto.Name,
            baseDto.Description,
            baseDto.Price,
            baseDto.CategoryId,
            baseDto.StockStatus,
            product.IsActive,
            product.CreatedAt,
            product.UpdatedAt,
            stock,
            product.Images.ToDTOAdminList()
        );
    }

    public static List<ProductResponseAdminDTO> ToDTOAdminList(this IEnumerable<Product> products, IEnumerable<StockProxyDTO> stocks)
    {
        var stockDict = stocks.ToDictionary(s => s.ProductId);

        return products.Select(p =>
        {
            var stock = stockDict.TryGetValue(p.Id, out var s) ? s : new StockProxyDTO(p.Id, 0);
            return p.ToDTOAdmin(stock);
        }).ToList();
    }

    public static PaginatedList<ProductResponseAdminDTO> ToPaginatedDTOAdmin(this PaginatedList<Product> paginatedProducts, IEnumerable<StockProxyDTO> stocks)
    {
        var dtoList = paginatedProducts.Items.ToDTOAdminList(stocks);

        return new PaginatedList<ProductResponseAdminDTO>(
            dtoList,
            paginatedProducts.TotalCount,
            paginatedProducts.Page,
            paginatedProducts.PageSize
        );
    }

    public static PaginatedList<ProductResponseDTO> ToPaginatedDTO(this PaginatedList<Product> paginatedProducts)
    {
        var dtoList = paginatedProducts.Items.ToDTOList();

        return new PaginatedList<ProductResponseDTO>(
            dtoList,
            paginatedProducts.TotalCount,
            paginatedProducts.Page,
            paginatedProducts.PageSize
        );
    }
}
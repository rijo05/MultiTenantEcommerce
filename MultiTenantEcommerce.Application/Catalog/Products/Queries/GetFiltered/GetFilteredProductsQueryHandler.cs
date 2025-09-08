using MultiTenantEcommerce.Application.Catalog.Products.DTOs;
using MultiTenantEcommerce.Application.Catalog.Products.Mappers;
using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Domain.Catalog.Interfaces;

namespace MultiTenantEcommerce.Application.Catalog.Products.Queries.GetFiltered;
public class GetFilteredProductsQueryHandler : IQueryHandler<GetFilteredProductsQuery, List<ProductResponseDTO>>
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ProductMapper _productMapper;

    public GetFilteredProductsQueryHandler(IProductRepository productRepository,
        ICategoryRepository categoryRepository,
        ProductMapper productMapper)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _productMapper = productMapper;
    }

    public async Task<List<ProductResponseDTO>> Handle(GetFilteredProductsQuery request, CancellationToken cancellationToken)
    {
        Guid? categoryId = request.CategoryId;

        if (!string.IsNullOrWhiteSpace(request.CategoryName))
        {
            var category = await _categoryRepository.GetByExactNameAsync(request.CategoryName);
            if (category is null)
                return new List<ProductResponseDTO>();

            categoryId = category.Id;
        }

        var products = await _productRepository.GetFilteredAsync(
            categoryId,
            request.Name,
            request.MinPrice,
            request.MaxPrice,
            request.IsActive,
            request.Page,
            request.PageSize,
            request.Sort);

        return _productMapper.ToProductResponseDTOList(products);
    }
}

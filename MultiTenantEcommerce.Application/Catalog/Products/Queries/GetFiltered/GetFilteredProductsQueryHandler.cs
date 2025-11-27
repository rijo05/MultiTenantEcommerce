using MultiTenantEcommerce.Application.Catalog.Products.DTOs.Products;
using MultiTenantEcommerce.Application.Catalog.Products.Mappers;
using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Common.Interfaces.Persistence;
using MultiTenantEcommerce.Domain.Catalog.Entities;
using MultiTenantEcommerce.Domain.Catalog.Interfaces;
using MultiTenantEcommerce.Domain.Inventory.Interfaces;

namespace MultiTenantEcommerce.Application.Catalog.Products.Queries.GetFiltered;
public class GetFilteredProductsQueryHandler : IQueryHandler<GetFilteredProductsQuery, List<IProductDTO>>
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IStockRepository _stockRepository;
    private readonly ProductMapper _productMapper;
    private readonly IFileStorageService _fileStorageService;

    public GetFilteredProductsQueryHandler(IProductRepository productRepository,
        ICategoryRepository categoryRepository,
        IStockRepository stockRepository,
        ProductMapper productMapper,
        IFileStorageService fileStorageService)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _stockRepository = stockRepository;
        _productMapper = productMapper;
        _fileStorageService = fileStorageService;
    }

    public async Task<List<IProductDTO>> Handle(GetFilteredProductsQuery request, CancellationToken cancellationToken)
    {
        Guid? categoryId = request.CategoryId;

        if (!string.IsNullOrWhiteSpace(request.CategoryName))
        {
            var category = await _categoryRepository.GetByExactNameAsync(request.CategoryName);
            if (category is null)
                return new List<IProductDTO>();

            categoryId = category.Id;
        }

        var products = (await _productRepository.GetFilteredAsync(
            categoryId,
            request.Name,
            request.MinPrice,
            request.MaxPrice,
            request.IsActive,
            request.Page,
            request.PageSize,
            request.Sort)).ToList();

        var stocks = await _stockRepository.GetBulkByIdsAsync(products.Select(x => x.Id).ToList());

        var imagesKeys = products.SelectMany(x => x.Images).Select(x => x.Key).ToList();

        var signedUrls = _fileStorageService.GetImageUrl(imagesKeys);

        return request.IsAdmin
            ? _productMapper.ToProductResponseAdminDTOList(products, stocks, signedUrls).Cast<IProductDTO>().ToList()
            : _productMapper.ToProductResponseDTOList(products, stocks, signedUrls).Cast<IProductDTO>().ToList();
    }
}

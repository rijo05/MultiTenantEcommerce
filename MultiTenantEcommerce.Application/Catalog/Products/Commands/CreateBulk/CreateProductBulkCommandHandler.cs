using MultiTenantEcommerce.Application.Catalog.Products.DTOs.Products;
using MultiTenantEcommerce.Application.Catalog.Products.Mappers;
using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Common.Interfaces.Persistence;
using MultiTenantEcommerce.Domain.Catalog.Entities;
using MultiTenantEcommerce.Domain.Catalog.Interfaces;
using MultiTenantEcommerce.Domain.Inventory.Entities;
using MultiTenantEcommerce.Domain.Inventory.Interfaces;
using MultiTenantEcommerce.Domain.ValueObjects;

namespace MultiTenantEcommerce.Application.Catalog.Products.Commands.CreateBulk;
public class CreateProductBulkCommandHandler : ICommandHandler<CreateProductBulkCommand, List<ProductResponseAdminDTO>>
{
    private readonly IProductRepository _productRepository;
    private readonly ITenantContext _tenantContext;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ProductMapper _productMapper;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IStockRepository _stockRepository;

    public CreateProductBulkCommandHandler(IProductRepository productRepository,
        ITenantContext tenantContext,
        IUnitOfWork unitOfWork,
        ProductMapper productMapper,
        ICategoryRepository categoryRepository,
        IStockRepository stockRepository)
    {
        _productRepository = productRepository;
        _tenantContext = tenantContext;
        _unitOfWork = unitOfWork;
        _productMapper = productMapper;
        _categoryRepository = categoryRepository;
        _stockRepository = stockRepository;
    }

    public async Task<List<ProductResponseAdminDTO>> Handle(CreateProductBulkCommand request, CancellationToken cancellationToken)
    {
        var categories = await _categoryRepository.GetByIdsAsync(request.Products.Select(x => x.CategoryId));

        if (categories.Count != request.Products.Select(x => x.CategoryId).Distinct().Count())
            throw new Exception("Some categories couldnt be find");

        var products = new List<Product>();
        var stocks = new List<Stock>();

        var categoryDict = categories.ToDictionary(x => x.Id);


        foreach (var cmd in request.Products)
        {
            if (!categoryDict.TryGetValue(cmd.CategoryId, out var category))
                throw new Exception($"Invalid category {cmd.CategoryId}");

            var product = new Product(
                _tenantContext.TenantId,
                cmd.Name,
                new Money(cmd.Price),
                category,
                cmd.Description,
                cmd.IsActive);

            var stock = new Stock(product,
                _tenantContext.TenantId,
                cmd.Quantity,
                cmd.MinimumQuantity);

            products.Add(product);
            stocks.Add(stock);
        }

        await _productRepository.AddBulkAsync(products);
        await _stockRepository.AddBulkAsync(stocks);
        await _unitOfWork.CommitAsync();

        return _productMapper.ToProductResponseAdminDTOList(products, stocks);
    }
}

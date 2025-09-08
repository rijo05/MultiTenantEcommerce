using MultiTenantEcommerce.Application.Catalog.Products.DTOs;
using MultiTenantEcommerce.Application.Catalog.Products.Mappers;
using MultiTenantEcommerce.Application.Common.Interfaces;
using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Common.Interfaces.Persistence;
using MultiTenantEcommerce.Domain.Catalog.Entities;
using MultiTenantEcommerce.Domain.Catalog.Interfaces;
using MultiTenantEcommerce.Domain.ValueObjects;

namespace MultiTenantEcommerce.Application.Catalog.Products.Commands.CreateBulk;
public class CreateProductBulkCommandHandler : ICommandHandler<CreateProductBulkCommand, List<ProductResponseDTO>>
{
    private readonly IProductRepository _productRepository;
    private readonly ITenantContext _tenantContext;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ProductMapper _productMapper;
    private readonly ICategoryRepository _categoryRepository;

    public CreateProductBulkCommandHandler(
        IProductRepository productRepository,
        ProductMapper productMapper,
        ITenantContext tenantContext,
        IUnitOfWork unitOfWork,
        ICategoryRepository categoryRepository)
    {
        _productRepository = productRepository;
        _productMapper = productMapper;
        _tenantContext = tenantContext;
        _unitOfWork = unitOfWork;
        _categoryRepository = categoryRepository;
    }

    public async Task<List<ProductResponseDTO>> Handle(CreateProductBulkCommand request, CancellationToken cancellationToken)
    {
        var categories = await _categoryRepository.GetByIdsAsync(request.Products.Select(x => x.CategoryId));

        if (categories.Count != request.Products.Select(x => x.CategoryId).Distinct().Count())
            throw new Exception("Some categories couldnt be find");

        var products = new List<Product>();

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

            products.Add(product);
        }

        await _productRepository.AddBulkAsync(products);
        await _unitOfWork.CommitAsync();

        return _productMapper.ToProductResponseDTOList(products);
    }
}

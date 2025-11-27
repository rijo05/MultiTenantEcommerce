using MultiTenantEcommerce.Application.Catalog.Products.DTOs.Products;
using MultiTenantEcommerce.Application.Catalog.Products.Mappers;
using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Common.Interfaces.Persistence;
using MultiTenantEcommerce.Domain.Catalog.Entities;
using MultiTenantEcommerce.Domain.Catalog.Interfaces;
using MultiTenantEcommerce.Domain.Inventory.Interfaces;
using System.Runtime.CompilerServices;

namespace MultiTenantEcommerce.Application.Catalog.Products.Commands.Update;
public class UpdateProductCommandHandler : ICommandHandler<UpdateProductCommand, ProductResponseAdminDTO>
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IStockRepository _stockRepository;
    private readonly IFileStorageService _fileStorageService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ProductMapper _productMapper;

    public UpdateProductCommandHandler(ICategoryRepository categoryRepository,
    IProductRepository productRepository,
    IStockRepository stockRepository,
    IFileStorageService fileStorageService,
    ProductMapper productMapper,
    IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _productRepository = productRepository;
        _productMapper = productMapper;
        _unitOfWork = unitOfWork;
        _fileStorageService = fileStorageService;
        _stockRepository = stockRepository;
    }

    public async Task<ProductResponseAdminDTO> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdIncluding(request.ProductId, x => x.Images)
            ?? throw new Exception("Product doesnt exist.");


        Category? category = null;

        if (request.CategoryId.HasValue)
        {
            category = await _categoryRepository.GetByIdAsync(request.CategoryId.Value)
                ?? throw new Exception("Category doesnt exist");
        }

        var stock = await _stockRepository.GetByProductIdAsync(product.Id)
            ?? throw new Exception("Stock not found. This shouldnt happen");

        var images = _fileStorageService.GetImageUrl(product.Images.Select(x => x.Key).ToList());

        product.UpdateProduct(request.Name,
            request.Description,
            request.Price,
            request.IsActive,
            request.CategoryId,
            category);

        await _unitOfWork.CommitAsync();
        return _productMapper.ToProductResponseAdminDTO(product, stock, images);
    }
}

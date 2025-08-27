using FluentValidation;
using MultiTenantEcommerce.Application.DTOs.Product;
using MultiTenantEcommerce.Application.Interfaces;
using MultiTenantEcommerce.Application.Mappers;
using MultiTenantEcommerce.Application.Validators.Common;
using MultiTenantEcommerce.Domain.Catalog.Entities;
using MultiTenantEcommerce.Domain.Catalog.Interfaces;
using MultiTenantEcommerce.Domain.Common.Interfaces;
using MultiTenantEcommerce.Domain.Inventory.Entities;
using MultiTenantEcommerce.Domain.Inventory.Interfaces;
using MultiTenantEcommerce.Domain.ValueObjects;
using MultiTenantEcommerce.Infrastructure.Persistence.Context;

namespace MultiTenantEcommerce.Application.Services;

public class ProductService : IProductService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IValidator<CreateProductDTO> _validatorCreate;
    private readonly IValidator<UpdateProductDTO> _validatorUpdate;
    private readonly ProductMapper _productMapper;
    private readonly HateoasLinkService _hateoasLinkService;
    private readonly TenantContext _tenantContext;
    private readonly IStockRepository _stockRepository;

    public ProductService(IUnitOfWork unitOfWork, 
        IProductRepository productRepository, 
        ICategoryRepository categoryRepository, 
        IValidator<UpdateProductDTO> validatorUpdate, 
        IValidator<CreateProductDTO> validatorCreator, 
        ProductMapper mapper, 
        HateoasLinkService hateoasLinkService, 
        TenantContext tenantContext, 
        IStockRepository stockRepository)
    {
        _unitOfWork = unitOfWork;
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _validatorCreate = validatorCreator;
        _validatorUpdate = validatorUpdate;
        _productMapper = mapper;
        _hateoasLinkService = hateoasLinkService;
        _tenantContext = tenantContext;
        _stockRepository = stockRepository;
    }

    public async Task<List<ProductResponseDTO>> GetAllProductsAsync()
    {
        var products = await _productRepository.GetAllAsync();
        return _productMapper.ToProductResponseDTOList(products);
    }

    public async Task<List<ProductResponseDTO>> GetFilteredProductsAsync(ProductFilterDTO productFilter)    
    {
        if (!string.IsNullOrWhiteSpace(productFilter.CategoryName))
        {
            var category = await _categoryRepository.GetByExactNameAsync(productFilter.CategoryName);
            if (category is null)
                return new List<ProductResponseDTO>();

            productFilter.CategoryId = category.Id;
        }

        var products = await _productRepository.GetFilteredAsync(productFilter.CategoryId,
            productFilter.Name,
            productFilter.MinPrice,
            productFilter.MaxPrice,
            productFilter.IsActive,
            productFilter.Page,
            productFilter.PageSize,
            productFilter.Sort);

        return _productMapper.ToProductResponseDTOList(products);
    }

    public async Task<ProductResponseDTO?> GetProductByIdAsync(Guid id)
    {
        var product = await _productRepository.GetByIdAsync(id);

        if (product is null)
            throw new Exception("Product not found");

        return _productMapper.ToProductResponseDTO(product);
    }

    public async Task<ProductResponseDTO?> GetProductBySKUAsync(string SKU)
    {
        if (string.IsNullOrWhiteSpace(SKU))
            throw new Exception("Please provide a valid SKU.");

        var product = await CheckIfSKUExists(SKU);

        if (product is null)
            throw new Exception("Product not found");

        return _productMapper.ToProductResponseDTO(product);
    }

    public async Task<ProductResponseDTO> AddProductAsync(CreateProductDTO createProductDTO)
    {
        await ValidationRules.ValidateAsync(createProductDTO, _validatorCreate);

        //Validar se a categoria existe
        await EnsureCategoryExists(createProductDTO.ProductDTO.CategoryId);


        var product = new Product(_tenantContext.TenantId, createProductDTO.ProductDTO.Name, new Money(createProductDTO.ProductDTO.Price),
            createProductDTO.ProductDTO.CategoryId,
            createProductDTO.ProductDTO.SKU,
            createProductDTO.ProductDTO.Description);

        var originalProduct = _productMapper.ToProductResponseDTO(product);

        var stock = new Stock(product, _tenantContext.TenantId, createProductDTO.stockDTO.Quantity, createProductDTO.stockDTO.MinimumQuantity);

        await _productRepository.AddAsync(product);
        await _stockRepository.AddAsync(stock);
      

        await _unitOfWork.CommitAsync();
        return _productMapper.ToProductResponseDTO(product);
    }

    public async Task<ProductResponseDTO> UpdateProductAsync(Guid id, UpdateProductDTO productDTO)
    {
        //Ver se o produto realmente existe
        var product = await EnsureProductExists(id);

        await ValidationRules.ValidateAsync(productDTO, _validatorUpdate);


        //Ver se a nova categoria vai ser alterada e se sim se existe
        if (productDTO.CategoryId.HasValue)
        {
            await EnsureCategoryExists(productDTO.CategoryId.Value);
            product.UpdateCategory(productDTO.CategoryId.Value);
        }

        if (!string.IsNullOrEmpty(productDTO.Name))
            product.UpdateName(productDTO.Name);

        if (productDTO.Description is not null)
        {
            if (productDTO.Description == "")
                product.ClearDescription();
            else
                product.UpdateDescription(productDTO.Description);
        }

        if (productDTO.Price.HasValue)
            product.ChangePrice(productDTO.Price.Value);

        if (productDTO.IsActive.HasValue)
            product.ChangeActive(productDTO.IsActive.Value);

        if (!string.IsNullOrEmpty(productDTO.SKU) && await CheckIfSKUExists(productDTO.SKU) is null)
            product.ChangeSKU(productDTO.SKU);


        await _unitOfWork.CommitAsync();
        return _productMapper.ToProductResponseDTO(product);
    }      

    public async Task DeleteProductAsync(Guid id)
    {
        //Ver se o produto existe
        var product = await EnsureProductExists(id);

        await _productRepository.DeleteAsync(product);
        await _unitOfWork.CommitAsync();
    }


    private async Task EnsureCategoryExists(Guid categoryId)
    {
        if (await _categoryRepository.GetByIdAsync(categoryId) is null)
            throw new Exception($"Category with ID {categoryId} does not exist.");
    }

    private async Task<Product?> EnsureProductExists(Guid id)
    {
        return await _productRepository.GetByIdAsync(id) ?? throw new Exception("Product doesn't exist.");
    }
    private async Task<Product?> CheckIfSKUExists(string SKU)
    {
        var product = await _productRepository.GetBySKUAsync(SKU);

        if (product is not null)
            return product;

        return null;
    }
}

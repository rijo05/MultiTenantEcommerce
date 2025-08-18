using FluentValidation;
using MultiTenantEcommerce.Application.DTOs.Category;
using MultiTenantEcommerce.Application.Interfaces;
using MultiTenantEcommerce.Application.Mappers;
using MultiTenantEcommerce.Domain.Entities;
using MultiTenantEcommerce.Domain.Interfaces;
using MultiTenantEcommerce.Infrastructure.Context;

namespace MultiTenantEcommerce.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IProductRepository _productRepository;
    private readonly CategoryMapper _categoryMapper;
    private readonly IValidator<CreateCategoryDTO> _validatorCreate;
    private readonly IValidator<UpdateCategoryDTO> _validatorUpdate;
    private readonly IUnitOfWork _unitOfWork;
    private readonly TenantContext _tenantContext;

    public CategoryService(ICategoryRepository categoryRepository, 
        IProductRepository productRepository, 
        CategoryMapper categoryMapper, 
        IValidator<CreateCategoryDTO> validatorCreate, 
        IValidator<UpdateCategoryDTO> validatorUpdate, 
        IUnitOfWork unitOfWork, 
        TenantContext tenantContext) 
    {
        _categoryRepository = categoryRepository;
        _productRepository = productRepository;
        _categoryMapper = categoryMapper;
        _validatorCreate = validatorCreate;
        _validatorUpdate = validatorUpdate; 
        _unitOfWork = unitOfWork;
        _tenantContext = tenantContext;
    }

    public async Task<List<CategoryResponseDTO>> GetFilteredCategoriesAsync(CategoryFilterDTO categoryFilter)
    {
        var categories = await _categoryRepository.GetFilteredAsync(categoryFilter.Name,
            categoryFilter.Description,
            categoryFilter.IsActive,
            categoryFilter.Page,
            categoryFilter.PageSize,
            categoryFilter.Sort);

        return _categoryMapper.ToCategoryResponseDTOList(categories);
    }

    public async Task<CategoryResponseDTO?> GetCategoryByIdAsync(Guid id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);

        if (category is null)
            throw new Exception($"Category with ID '{id}' not found.");

        return _categoryMapper.ToCategoryResponseDTO(category);
    }

    public async Task<CategoryResponseDTO> AddCategoryAsync(CreateCategoryDTO categoryDTO)
    {
        //Valida os dados
        var validationResult = await _validatorCreate.ValidateAsync(categoryDTO);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);


        //Ve se o nome da categoria já existe
        if (await _categoryRepository.GetByExactNameAsync(categoryDTO.Name) is not null)
            throw new ValidationException($"Category name '{categoryDTO.Name}' already exists.");


        var category = new Category(_tenantContext.TenantId, categoryDTO.Name, categoryDTO.Description);
        await _categoryRepository.AddAsync(category);
        await _unitOfWork.CommitAsync();
        return _categoryMapper.ToCategoryResponseDTO(category);
        
    }
    public async Task<CategoryResponseDTO> UpdateCategoryAsync(Guid id, UpdateCategoryDTO categoryDTO)
    {
        //Ve se o Id realmente pertence a uma categoria
        var category = await EnsureCategoryExists(id);

        //Valida os dados
        var validationResult = await _validatorUpdate.ValidateAsync(categoryDTO);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        //Ve se o nome da categoria já existe
        if (!string.IsNullOrWhiteSpace(categoryDTO.Name) && categoryDTO.Name != category.Name)
        {
            if (await _categoryRepository.GetByExactNameAsync(categoryDTO.Name) is not null)
                throw new Exception($"Category name '{categoryDTO.Name}' already exists.");
        }


        if (categoryDTO.IsActive.HasValue)
            category.SetActive(categoryDTO.IsActive.Value);

        if (categoryDTO.Description is not null)
        {
            if (categoryDTO.Description == "")
                category.ClearDescription();
            else
                category.UpdateDescription(categoryDTO.Description);
        }

        if (!string.IsNullOrWhiteSpace(categoryDTO.Name))
            category.UpdateName(categoryDTO.Name);


        await _unitOfWork.CommitAsync();
        return _categoryMapper.ToCategoryResponseDTO(category);
    }

    public async Task DeleteCategoryAsync(Guid id)
    {
        var category = await EnsureCategoryExists(id);

        var products = await _productRepository.GetByCategoryIdAsync(id);
        if (products.Any())
            throw new Exception($"Cannot delete category because it has {products.Count} products.");

        await _categoryRepository.DeleteAsync(category);
        await _unitOfWork.CommitAsync();
    }


    private async Task<Category?> EnsureCategoryExists(Guid id)
    {
        return await _categoryRepository.GetByIdAsync(id) ?? throw new Exception("Category doesn't exist.");
    }
}

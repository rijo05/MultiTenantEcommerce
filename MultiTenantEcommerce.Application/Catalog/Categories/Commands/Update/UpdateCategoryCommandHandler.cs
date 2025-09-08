using MultiTenantEcommerce.Application.Catalog.Categories.DTOs;
using MultiTenantEcommerce.Application.Catalog.Categories.Mappers;
using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Common.Interfaces.Persistence;
using MultiTenantEcommerce.Domain.Catalog.Interfaces;

namespace MultiTenantEcommerce.Application.Catalog.Categories.Commands.Update;
public class UpdateCategoryCommandHandler : ICommandHandler<UpdateCategoryCommand, CategoryResponseDTO>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly CategoryMapper _categoryMapper;

    public UpdateCategoryCommandHandler(
        ICategoryRepository categoryRepository,
        IUnitOfWork unitOfWork,
        CategoryMapper categoryMapper)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
        _categoryMapper = categoryMapper;
    }

    public async Task<CategoryResponseDTO> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(request.CategoryId) ?? throw new Exception("Category doesnt exist.");

        if (!string.IsNullOrWhiteSpace(request.Name) && request.Name != category.Name)
        {
            if (await _categoryRepository.GetByExactNameAsync(request.Name) is not null)
                throw new Exception($"Category name '{request.Name}' already exists.");
        }

        category.UpdateCategory(request.Name, request.Description, request.IsActive);

        await _unitOfWork.CommitAsync();
        return _categoryMapper.ToCategoryResponseDTO(category);
    }
}

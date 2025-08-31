using MultiTenantEcommerce.Application.Catalog.Categories.DTOs;
using MultiTenantEcommerce.Application.Catalog.Categories.Mappers;
using MultiTenantEcommerce.Application.Common.Interfaces;
using MultiTenantEcommerce.Application.Common.Validators;
using MultiTenantEcommerce.Domain.Catalog.Interfaces;
using MultiTenantEcommerce.Domain.Common.Interfaces;
using MultiTenantEcommerce.Infrastructure.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Application.Catalog.Categories.Commands.Update;
public class UpdateCategoryCommandHandler : ICommandHandler<UpdateCategoryCommand, CategoryResponseDTO>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly CategoryMapper _categoryMapper;
    private readonly TenantContext _tenantContext;

    public UpdateCategoryCommandHandler(
        ICategoryRepository categoryRepository,
        IUnitOfWork unitOfWork,
        CategoryMapper categoryMapper,
        TenantContext tenantContext)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
        _categoryMapper = categoryMapper;
        _tenantContext = tenantContext;
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

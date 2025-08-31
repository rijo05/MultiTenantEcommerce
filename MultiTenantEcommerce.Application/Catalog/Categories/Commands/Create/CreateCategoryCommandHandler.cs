using MultiTenantEcommerce.Application.Catalog.Categories.DTOs;
using MultiTenantEcommerce.Application.Catalog.Categories.Mappers;
using MultiTenantEcommerce.Application.Common.Interfaces;
using MultiTenantEcommerce.Domain.Catalog.Interfaces;
using MultiTenantEcommerce.Domain.Common.Interfaces;
using MultiTenantEcommerce.Infrastructure.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Application.Catalog.Categories.Commands.Create;
public class CreateCategoryCommandHandler : ICommandHandler<CreateCategoryCommand, CategoryResponseDTO>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly CategoryMapper _categoryMapper;
    private readonly TenantContext _tenantContext;

    public CreateCategoryCommandHandler(
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

    public async Task<CategoryResponseDTO> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            throw new Exception("Category name must be provided.");

        if (await _categoryRepository.GetByExactNameAsync(request.Name) is not null)
            throw new Exception("A category with that name already exists.");

        var category = new Domain.Catalog.Entities.Category(
            _tenantContext.TenantId,
            request.Name,
            request.Description
            );

        await _categoryRepository.AddAsync(category);
        await _unitOfWork.CommitAsync();

        return _categoryMapper.ToCategoryResponseDTO(category);
    }
}

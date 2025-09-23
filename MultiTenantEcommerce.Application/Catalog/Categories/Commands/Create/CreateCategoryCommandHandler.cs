using MultiTenantEcommerce.Application.Catalog.Categories.DTOs;
using MultiTenantEcommerce.Application.Catalog.Categories.Mappers;
using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Common.Interfaces.Persistence;
using MultiTenantEcommerce.Domain.Catalog.Entities;
using MultiTenantEcommerce.Domain.Catalog.Interfaces;

namespace MultiTenantEcommerce.Application.Catalog.Categories.Commands.Create;
public class CreateCategoryCommandHandler : ICommandHandler<CreateCategoryCommand, CategoryResponseAdminDTO>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly CategoryMapper _categoryMapper;
    private readonly ITenantContext _tenantContext;

    public CreateCategoryCommandHandler(
        ICategoryRepository categoryRepository,
        IUnitOfWork unitOfWork,
        CategoryMapper categoryMapper,
        ITenantContext tenantContext)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
        _categoryMapper = categoryMapper;
        _tenantContext = tenantContext;
    }

    public async Task<CategoryResponseAdminDTO> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            throw new Exception("Category name must be provided.");

        if (await _categoryRepository.GetByExactNameAsync(request.Name) is not null)
            throw new Exception("A category with that name already exists.");

        var category = new Category(
            _tenantContext.TenantId,
            request.Name,
            request.Description
            );

        await _categoryRepository.AddAsync(category);
        await _unitOfWork.CommitAsync();

        return _categoryMapper.ToCategoryResponseAdminDTO(category);
    }
}

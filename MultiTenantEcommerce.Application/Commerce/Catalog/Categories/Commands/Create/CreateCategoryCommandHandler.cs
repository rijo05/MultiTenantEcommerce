using MultiTenantEcommerce.Application.Commerce.Catalog.Categories.Common.DTOs;
using MultiTenantEcommerce.Application.Commerce.Catalog.Categories.Common.Mappers;
using MultiTenantEcommerce.Domain.Commerce.Catalog.Entities;
using MultiTenantEcommerce.Domain.Commerce.Catalog.Interfaces;
using MultiTenantEcommerce.Shared.Application.CQRS;
using MultiTenantEcommerce.Shared.Infrastructure.Persistence;

namespace MultiTenantEcommerce.Application.Commerce.Catalog.Categories.Commands.Create;

public class CreateCategoryCommandHandler : ICommandHandler<CreateCategoryCommand, CategoryResponseAdminDTO>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly ITenantContext _tenantContext;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCategoryCommandHandler(
        ICategoryRepository categoryRepository,
        IUnitOfWork unitOfWork,
        ITenantContext tenantContext)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
        _tenantContext = tenantContext;
    }

    public async Task<CategoryResponseAdminDTO> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        if (await _categoryRepository.GetByExactNameAsync(request.Name) is not null)
            throw new Exception("A category with that name already exists.");

        var category = new Category(
            _tenantContext.TenantId,
            request.Name,
            request.Description
        );

        await _categoryRepository.AddAsync(category);
        await _unitOfWork.CommitAsync();

        return category.ToDTOAdmin();
    }
}
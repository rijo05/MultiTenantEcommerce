using MediatR;
using MultiTenantEcommerce.Application.Commerce.Catalog.Categories.Common.DTOs;
using MultiTenantEcommerce.Application.Commerce.Catalog.Categories.Common.Mappers;
using MultiTenantEcommerce.Domain.Commerce.Catalog.Interfaces;
using MultiTenantEcommerce.Shared.Application.CQRS;
using MultiTenantEcommerce.Shared.Infrastructure.Persistence;

namespace MultiTenantEcommerce.Application.Commerce.Catalog.Categories.Commands.Update;

public class UpdateCategoryCommandHandler : ICommandHandler<UpdateCategoryCommand, Unit>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCategoryCommandHandler(
        ICategoryRepository categoryRepository,
        IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(UpdateCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(request.CategoryId)
                       ?? throw new Exception("Category doesnt exist.");

        if (!string.IsNullOrWhiteSpace(request.Name) && request.Name != category.Name)
            if (await _categoryRepository.GetByExactNameAsync(request.Name) is not null)
                throw new Exception($"Category name '{request.Name}' already exists.");

        category.UpdateCategory(request.Name, request.Description, request.IsActive);

        await _unitOfWork.CommitAsync();
        return Unit.Value;
    }
}
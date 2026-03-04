using MediatR;
using MultiTenantEcommerce.Domain.Commerce.Catalog.Interfaces;
using MultiTenantEcommerce.Shared.Application.CQRS;
using MultiTenantEcommerce.Shared.Infrastructure.Persistence;

namespace MultiTenantEcommerce.Application.Commerce.Catalog.Categories.Commands.Delete;

public class DeleteCategoryCommandHandler : ICommandHandler<DeleteCategoryCommand, Unit>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCategoryCommandHandler(
        ICategoryRepository categoryRepository,
        IProductRepository productRepository,
        IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(request.Id)
                       ?? throw new Exception("Category doesnt exist.");

        if (await _productRepository.HasProductsInCategoryAsync(category.Id))
            throw new Exception("Cant delete a category with active products");

        await _categoryRepository.DeleteAsync(category);
        await _unitOfWork.CommitAsync();

        return Unit.Value;
    }
}
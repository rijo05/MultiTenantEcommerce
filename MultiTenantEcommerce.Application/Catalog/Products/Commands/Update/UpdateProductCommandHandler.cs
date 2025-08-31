using MultiTenantEcommerce.Application.Catalog.Products.DTOs;
using MultiTenantEcommerce.Application.Catalog.Products.Mappers;
using MultiTenantEcommerce.Application.Common.Interfaces;
using MultiTenantEcommerce.Domain.Catalog.Interfaces;
using MultiTenantEcommerce.Domain.Common.Interfaces;
using MultiTenantEcommerce.Domain.Inventory.Interfaces;
using MultiTenantEcommerce.Infrastructure.Persistence.Context;
using MultiTenantEcommerce.Infrastructure.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Application.Catalog.Products.Commands.Update;
public class UpdateProductCommandHandler : ICommandHandler<UpdateProductCommand, ProductResponseDTO>
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ProductMapper _productMapper;

    public UpdateProductCommandHandler(ICategoryRepository categoryRepository,
    IProductRepository productRepository,
    ProductMapper productMapper,
    IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _productRepository = productRepository;
        _productMapper = productMapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ProductResponseDTO> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId) 
            ?? throw new Exception("Product doesnt exist.");

        if (request.CategoryId.HasValue)
        {
            var categoria = await _categoryRepository.GetByIdAsync(request.CategoryId.Value) 
                ?? throw new Exception("Category doesnt exist");
        }

        product.UpdateProduct(request.Name, 
            request.Description, 
            request.Price, 
            request.IsActive, 
            request.CategoryId);

        await _unitOfWork.CommitAsync();
        return _productMapper.ToProductResponseDTO(product);
    }
}

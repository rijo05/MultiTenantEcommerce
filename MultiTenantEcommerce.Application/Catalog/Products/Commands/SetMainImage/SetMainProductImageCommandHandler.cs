using MultiTenantEcommerce.Application.Catalog.Products.DTOs;
using MultiTenantEcommerce.Application.Catalog.Products.DTOs.Products;
using MultiTenantEcommerce.Application.Catalog.Products.Mappers;
using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Common.Interfaces.Persistence;
using MultiTenantEcommerce.Domain.Catalog.Entities;
using MultiTenantEcommerce.Domain.Catalog.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Application.Catalog.Products.Commands.SetMainImage;
public class SetMainProductImageCommandHandler : ICommandHandler<SetMainProductImageCommand, ProductResponseWithoutStockAdminDTO>
{
    private readonly IProductRepository _productRepository;
    private readonly ProductMapper _productMapper;
    private readonly IFileStorageService _fileStorageService;
    public async Task<ProductResponseWithoutStockAdminDTO> Handle(SetMainProductImageCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdIncluding(request.ProductId, x => x.Images)
            ?? throw new Exception("Product doesnt exist");

        if (!product.Images.Select(x => x.Key).Contains(request.Key))
            throw new Exception("Image doesnt exist");

        product.MarkAsMain(request.Key);

        return _productMapper.ToProductResponseWithoutStockDTO(product);
    }
}

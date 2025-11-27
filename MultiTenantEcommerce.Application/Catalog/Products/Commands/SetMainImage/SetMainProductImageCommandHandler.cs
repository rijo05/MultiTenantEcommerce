using MultiTenantEcommerce.Application.Catalog.Products.DTOs;
using MultiTenantEcommerce.Application.Catalog.Products.DTOs.Products;
using MultiTenantEcommerce.Application.Catalog.Products.Mappers;
using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Common.Interfaces.Persistence;
using MultiTenantEcommerce.Domain.Catalog.Entities;
using MultiTenantEcommerce.Domain.Catalog.Interfaces;
using MultiTenantEcommerce.Domain.Inventory.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Application.Catalog.Products.Commands.SetMainImage;
public class SetMainProductImageCommandHandler : ICommandHandler<SetMainProductImageCommand, ProductResponseAdminDTO>
{
    private readonly IProductRepository _productRepository;
    private readonly ProductMapper _productMapper;
    private readonly IFileStorageService _fileStorageService;
    private readonly IStockRepository _stockRepository;

    public SetMainProductImageCommandHandler(IProductRepository productRepository, 
        ProductMapper productMapper, 
        IFileStorageService fileStorageService, 
        IStockRepository stockRepository)
    {
        _productRepository = productRepository;
        _productMapper = productMapper;
        _fileStorageService = fileStorageService;
        _stockRepository = stockRepository;
    }

    public async Task<ProductResponseAdminDTO> Handle(SetMainProductImageCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdIncluding(request.ProductId, x => x.Images)
            ?? throw new Exception("Product doesnt exist");

        if (!product.Images.Select(x => x.Key).Contains(request.Key))
            throw new Exception("Image doesnt exist");

        var stock = await _stockRepository.GetByProductIdAsync(product.Id)
            ?? throw new Exception("Stock not found. This shouldnt happen");

        var images = _fileStorageService.GetImageUrl(product.Images.Select(x => x.Key).ToList());

        product.MarkAsMain(request.Key);

        return _productMapper.ToProductResponseAdminDTO(product, stock, images);
    }
}

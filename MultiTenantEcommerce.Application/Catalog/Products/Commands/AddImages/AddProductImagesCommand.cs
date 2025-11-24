using MultiTenantEcommerce.Application.Catalog.Products.DTOs.Images;
using MultiTenantEcommerce.Application.Common.DTOs;
using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;

namespace MultiTenantEcommerce.Application.Catalog.Products.Commands.AddImages;
public record AddProductImagesCommand(
    Guid ProductId,
    List<ProductImageMetadataDTO> Metadata) : ICommand<List<PresignedUpload>>;
using MultiTenantEcommerce.Domain.Commerce.Catalog.ValueObjects;
using MultiTenantEcommerce.Shared.Application.CQRS;
using MultiTenantEcommerce.Shared.Integration.DTOs;
using System.Drawing;

namespace MultiTenantEcommerce.Application.Commerce.Catalog.Products.Commands.ManageImages;

public record AddProductImagesCommand(
    Guid ProductId,
    List<ImageMetadata> Metadata) : ICommand<List<PresignedUpload>>;
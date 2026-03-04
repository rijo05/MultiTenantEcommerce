using MultiTenantEcommerce.Shared.Infrastructure.Messaging;
using MultiTenantEcommerce.Shared.Infrastructure.Services;
using MultiTenantEcommerce.Shared.Integration.Events.Commerce;

namespace MultiTenantEcommerce.Application.Commerce.Catalog.Products.EventHandlers.Integration;

public class DeleteImageOnProductImageDeletedEventHandler : IAsyncHandler<ProductImageDeletedIntegrationEvent>
{
    private readonly IFileStorageService _fileStorageService;

    public DeleteImageOnProductImageDeletedEventHandler(IFileStorageService fileStorageService)
    {
        _fileStorageService = fileStorageService;
    }

    public async Task HandleAsync(ProductImageDeletedIntegrationEvent evt)
    {
        try
        {
            await _fileStorageService.DeleteImageUrl(evt.ImageKey);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}

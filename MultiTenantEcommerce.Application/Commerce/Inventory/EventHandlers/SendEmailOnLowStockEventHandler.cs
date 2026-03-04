using MultiTenantEcommerce.Domain.Commerce.Catalog.Interfaces;
using MultiTenantEcommerce.Domain.Logistics.Inventory.Events;
using MultiTenantEcommerce.Domain.Platform.Tenancy.Interfaces.Repositories;

namespace MultiTenantEcommerce.Application.Commerce.Inventory.EventHandlers;

public class SendEmailOnLowStockEventHandler : IEventHandler<LowStockEvent>
{
    private readonly IEmailQueueRepository _emailQueueRepository;
    private readonly ITenantMemberRepository _employeeRepository;
    private readonly IProductRepository _productRepository;

    public SendEmailOnLowStockEventHandler(IEmailQueueRepository emailQueueRepository,
        ITenantMemberRepository employeeRepository,
        IProductRepository productRepository)
    {
        _emailQueueRepository = emailQueueRepository;
        _employeeRepository = employeeRepository;
        _productRepository = productRepository;
    }

    public async Task HandleAsync(LowStockEvent domainEvent)
    {
        var owner = await _employeeRepository.GetOwnerOfTenant(domainEvent.TenantId)
                    ?? throw new Exception("Something went wrong");

        var product = await _productRepository.GetByIdAsync(domainEvent.ProductId)
                      ?? throw new Exception("Something went wrong");

        var metadata = new Dictionary<string, string>
        {
            [EmailMetadataKeys.ProductId] = domainEvent.ProductId.ToString(),
            [EmailMetadataKeys.ProductName] = product.Name,
            [EmailMetadataKeys.CurrentQuantity] = domainEvent.CurrentQuantity.ToString(),
            [EmailMetadataKeys.MinimumQuantity] = domainEvent.MinimumQuantity.ToString()
        };

        var email = new EmailJobDataDTO(
            Guid.Empty,
            domainEvent.TenantId,
            "My Platform",
            owner.Email.Value,
            EmailTemplateNames.LowStock,
            domainEvent.EventPriority,
            metadata,
            ""
        );

        await _emailQueueRepository.EnqueueEmailAsync(email);
    }
}
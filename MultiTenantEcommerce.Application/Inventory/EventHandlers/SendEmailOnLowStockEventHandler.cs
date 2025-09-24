using MultiTenantEcommerce.Application.Common.DTOs;
using MultiTenantEcommerce.Application.Common.Helpers.EmailKeys;
using MultiTenantEcommerce.Application.Common.Interfaces.Persistence;
using MultiTenantEcommerce.Domain.Catalog.Interfaces;
using MultiTenantEcommerce.Domain.Enums;
using MultiTenantEcommerce.Domain.Inventory.Events;
using MultiTenantEcommerce.Domain.Users.Interfaces;

namespace MultiTenantEcommerce.Application.Inventory.EventHandlers;
public class SendEmailOnLowStockEventHandler : IEventHandler<LowStockEvent>
{
    private readonly IEmailQueueRepository _emailQueueRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IProductRepository _productRepository;

    public SendEmailOnLowStockEventHandler(IEmailQueueRepository emailQueueRepository,
        IEmployeeRepository employeeRepository,
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
            [EmailMetadataKeys.MinimumQuantity] = domainEvent.MinimumQuantity.ToString(),
        };

        var email = new EmailJobDataDTO(
            Guid.Empty,
            domainEvent.TenantId,
            owner.Email.Value,
            EmailTemplateNames.LowStockEvent,
            metadata,
            ""
        );

        await _emailQueueRepository.EnqueueEmailAsync(email);
    }
}

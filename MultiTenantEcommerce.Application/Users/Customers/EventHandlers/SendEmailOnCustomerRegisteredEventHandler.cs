using MultiTenantEcommerce.Application.Common.DTOs;
using MultiTenantEcommerce.Application.Common.Helpers.EmailKeys;
using MultiTenantEcommerce.Application.Common.Interfaces.Persistence;
using MultiTenantEcommerce.Domain.Enums;
using MultiTenantEcommerce.Domain.Tenants.Interfaces;
using MultiTenantEcommerce.Domain.Users.Events;
using MultiTenantEcommerce.Domain.Users.Interfaces;

namespace MultiTenantEcommerce.Application.Users.Customers.EventHandlers;
public class SendEmailOnCustomerRegisteredEventHandler : IEventHandler<CustomerRegisteredEvent>
{
    private readonly IEmailQueueRepository _emailQueueRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly ITenantRepository _tenantRepository;

    public SendEmailOnCustomerRegisteredEventHandler(IEmailQueueRepository emailQueueRepository,
        ICustomerRepository customerRepository,
        ITenantRepository tenantRepository)
    {
        _emailQueueRepository = emailQueueRepository;
        _customerRepository = customerRepository;
        _tenantRepository = tenantRepository;
    }

    public async Task HandleAsync(CustomerRegisteredEvent domainEvent)
    {
        var customer = await _customerRepository.GetByIdAsync(domainEvent.CustomerId)
            ?? throw new Exception("Customer not found");

        var tenant = await _tenantRepository.GetByIdAsync(domainEvent.TenantId)
            ?? throw new Exception("Tenant not found");

        var metadata = new Dictionary<string, string>
        {
            [EmailMetadataKeys.CustomerName] = customer.Name,
            [EmailMetadataKeys.TenantName] = tenant.Name
        };

        var email = new EmailJobDataDTO(
            Guid.Empty,
            domainEvent.TenantId,
            customer.Email.Value,
            EmailTemplateNames.CustomerRegistered,
            metadata,
            /*tenant.Email.Value*/ "boasboas@gmail.com"
        );

        await _emailQueueRepository.EnqueueEmailAsync(email);
    }
}


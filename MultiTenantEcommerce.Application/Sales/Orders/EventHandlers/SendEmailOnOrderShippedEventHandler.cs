using MultiTenantEcommerce.Application.Common.DTOs;
using MultiTenantEcommerce.Application.Common.Helpers.EmailKeys;
using MultiTenantEcommerce.Application.Common.Interfaces.Persistence;
using MultiTenantEcommerce.Domain.Enums;
using MultiTenantEcommerce.Domain.Sales.Orders.Events;
using MultiTenantEcommerce.Domain.Sales.Orders.Interfaces;
using MultiTenantEcommerce.Domain.Tenants.Interfaces;
using MultiTenantEcommerce.Domain.Users.Interfaces;

namespace MultiTenantEcommerce.Application.Sales.Orders.EventHandlers;
public class SendEmailOnOrderShippedEventHandler : IEventHandler<OrderShippedEvent>
{
    private readonly IEmailQueueRepository _emailQueueRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly ITenantRepository _tenantRepository;

    public SendEmailOnOrderShippedEventHandler(
        IEmailQueueRepository emailQueueRepository,
        IOrderRepository orderRepository,
        ICustomerRepository customerRepository,
        ITenantRepository tenantRepository)
    {
        _emailQueueRepository = emailQueueRepository;
        _orderRepository = orderRepository;
        _customerRepository = customerRepository;
        _tenantRepository = tenantRepository;
    }

    public async Task HandleAsync(OrderShippedEvent domainEvent)
    {
        var order = await _orderRepository.GetByIdAsync(domainEvent.OrderId)
            ?? throw new Exception("Order not found");

        var customer = await _customerRepository.GetByIdAsync(order.CustomerId)
            ?? throw new Exception("Customer not found");

        var tenant = await _tenantRepository.GetByIdAsync(domainEvent.TenantId)
            ?? throw new Exception("Tenant not found");

        var metadata = new Dictionary<string, string>
        {
            [EmailMetadataKeys.CustomerName] = customer.Name,
            [EmailMetadataKeys.OrderId] = order.Id.ToString(),
            [EmailMetadataKeys.TrackingNumber] = "12345", //######## TODO
            [EmailMetadataKeys.TrackingLink] = "www.ctt.track/1234",
            [EmailMetadataKeys.CarrierName] = "CTT",
            [EmailMetadataKeys.DeliveryDate] = DateTime.UtcNow.ToString(),
            [EmailMetadataKeys.TenantName] = tenant.Name
        };

        var email = new EmailJobDataDTO(
            Guid.Empty,
            domainEvent.TenantId,
            tenant.Name,
            customer.Email.Value,
            EmailTemplateNames.OrderShipped,
            domainEvent.EventPriority,
            metadata,
            tenant.Email.Value
        );

        await _emailQueueRepository.EnqueueEmailAsync(email);
    }
}

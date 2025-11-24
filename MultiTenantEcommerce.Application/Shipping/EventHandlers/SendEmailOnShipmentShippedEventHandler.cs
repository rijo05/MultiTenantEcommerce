using MultiTenantEcommerce.Application.Common.DTOs;
using MultiTenantEcommerce.Application.Common.Helpers.EmailKeys;
using MultiTenantEcommerce.Application.Common.Interfaces.Persistence;
using MultiTenantEcommerce.Application.Shipping.Interfaces;
using MultiTenantEcommerce.Domain.Enums;
using MultiTenantEcommerce.Domain.Sales.Orders.Interfaces;
using MultiTenantEcommerce.Domain.Shipping.Events;
using MultiTenantEcommerce.Domain.Tenants.Interfaces;
using MultiTenantEcommerce.Domain.Users.Interfaces;

namespace MultiTenantEcommerce.Application.Sales.Orders.EventHandlers;
public class SendEmailOnShipmentShippedEventHandler : IEventHandler<ShipmentShippedEvent>
{
    private readonly IEmailQueueRepository _emailQueueRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly ITenantRepository _tenantRepository;
    private readonly IShippingService _shippingService;

    public SendEmailOnShipmentShippedEventHandler(
        IEmailQueueRepository emailQueueRepository,
        IOrderRepository orderRepository,
        ICustomerRepository customerRepository,
        ITenantRepository tenantRepository,
        IShippingService shippingService)
    {
        _emailQueueRepository = emailQueueRepository;
        _orderRepository = orderRepository;
        _customerRepository = customerRepository;
        _tenantRepository = tenantRepository;
        _shippingService = shippingService;
    }

    public async Task HandleAsync(ShipmentShippedEvent domainEvent)
    {
        var order = await _orderRepository.GetByIdIncluding(domainEvent.OrderId, x => x.Shipment!)
            ?? throw new Exception("Order not found");

        if (order.Shipment is null)
            throw new Exception("Shipment doesnt exist for this order");

        var customer = await _customerRepository.GetByIdAsync(order.CustomerId)
            ?? throw new Exception("Customer not found");

        var tenant = await _tenantRepository.GetByIdAsync(domainEvent.TenantId)
            ?? throw new Exception("Tenant not found");

        var metadata = new Dictionary<string, string>
        {
            [EmailMetadataKeys.CustomerName] = customer.Name,
            [EmailMetadataKeys.OrderId] = order.Id.ToString(),
            [EmailMetadataKeys.TrackingNumber] = order.Shipment.TrackingNumber,
            [EmailMetadataKeys.TrackingLink] = $"{order.Shipment.Carrier.ToString()}/track/{order.Shipment.TrackingNumber}",
            [EmailMetadataKeys.CarrierName] = order.Shipment.Carrier.ToString(),
            [EmailMetadataKeys.DeliveryDate] = order.Shipment.EstimatedDeliveryDate.ToString(),
            [EmailMetadataKeys.TenantName] = tenant.Name
        };

        var email = new EmailJobDataDTO(
            Guid.Empty,
            domainEvent.TenantId,
            tenant.Name,
            customer.Email.Value,
            EmailTemplateNames.ShipmentShipped,
            domainEvent.EventPriority,
            metadata,
            tenant.Email.Value
        );

        await _emailQueueRepository.EnqueueEmailAsync(email);
    }
}

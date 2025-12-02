using MultiTenantEcommerce.Application.Common.DTOs;
using MultiTenantEcommerce.Application.Common.Helpers.EmailKeys;
using MultiTenantEcommerce.Application.Common.Interfaces.Persistence;
using MultiTenantEcommerce.Domain.Enums;
using MultiTenantEcommerce.Domain.Payment.Interfaces;
using MultiTenantEcommerce.Domain.Sales.Orders.Events;
using MultiTenantEcommerce.Domain.Sales.Orders.Interfaces;
using MultiTenantEcommerce.Domain.Tenants.Interfaces;
using MultiTenantEcommerce.Domain.Users.Interfaces;

namespace MultiTenantEcommerce.Application.Sales.Orders.EventHandlers;
public class SendEmailOnOrderPaidEventHandler : IEventHandler<OrderPaidEvent>
{
    private readonly IEmailQueueRepository _emailQueueRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly ITenantRepository _tenantRepository;
    private readonly IOrderPaymentRepository _orderPaymentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SendEmailOnOrderPaidEventHandler(
        IEmailQueueRepository emailQueueRepository,
        IOrderRepository orderRepository,
        ICustomerRepository customerRepository,
        ITenantRepository tenantRepository,
        IOrderPaymentRepository orderPaymentRepository,
        IUnitOfWork unitOfWork)
    {
        _emailQueueRepository = emailQueueRepository;
        _orderRepository = orderRepository;
        _customerRepository = customerRepository;
        _tenantRepository = tenantRepository;
        _orderPaymentRepository = orderPaymentRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task HandleAsync(OrderPaidEvent domainEvent)
    {
        var order = await _orderRepository.GetByIdAsync(domainEvent.OrderId)
            ?? throw new Exception("Order not found");

        var payment = await _orderPaymentRepository.GetByOrderId(domainEvent.OrderId)
            ?? throw new Exception("payment not found, shouldnt happen");

        var customer = await _customerRepository.GetByIdAsync(order.CustomerId)
            ?? throw new Exception("Customer not found");

        var tenant = await _tenantRepository.GetByIdAsync(domainEvent.TenantId)
            ?? throw new Exception("Tenant not found");

        var metadata = new Dictionary<string, string>
        {
            [EmailMetadataKeys.CustomerName] = customer.Name,
            [EmailMetadataKeys.OrderId] = order.Id.ToString(),
            [EmailMetadataKeys.AmountPaid] = order.Price.Value.ToString() + "€",
            [EmailMetadataKeys.PaymentMethod] = payment.PaymentMethod.ToString(),
            [EmailMetadataKeys.BillingAddress] = order.Address.ToString(),
            //[EmailMetadataKeys.ItemsHtml] = string.Join("", order.Items.Select(item => $"<tr><td>{item.ProductName}</td><td>{item.Quantity}</td><td>{item.UnitPrice:C}</td></tr>")),
            [EmailMetadataKeys.TenantName] = tenant.Name
        };

        var email = new EmailJobDataDTO(
            Guid.Empty,
            domainEvent.TenantId,
            tenant.Name,
            customer.Email.Value,
            EmailTemplateNames.OrderPaid,
            domainEvent.EventPriority,
            metadata,
            tenant.Email.Value
        );

        await _emailQueueRepository.EnqueueEmailAsync(email);
    }
}

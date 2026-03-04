using MultiTenantEcommerce.Domain.Commerce.Customers.Interfaces;
using MultiTenantEcommerce.Domain.Commerce.Sales.Orders.Events;
using MultiTenantEcommerce.Domain.Commerce.Sales.Orders.Interfaces;
using MultiTenantEcommerce.Domain.Commerce.Sales.Payment.Interfaces;
using MultiTenantEcommerce.Domain.Platform.Tenancy.Interfaces.Repositories;

namespace MultiTenantEcommerce.Application.Commerce.Sales.Orders.EventHandlers;

public class SendEmailOnOrderPaidEventHandler : EmailHandlerBase<OrderPaidEvent>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IEmailQueueRepository _emailQueueRepository;
    private readonly IOrderPaymentRepository _orderPaymentRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly ITenantRepository _tenantRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SendEmailOnOrderPaidEventHandler(
        IEmailQueueRepository emailQueueRepository,
        IOrderRepository orderRepository,
        ICustomerRepository customerRepository,
        ITenantRepository tenantRepository,
        IOrderPaymentRepository orderPaymentRepository,
        IUnitOfWork unitOfWork,
        IEmailSender sender,
        ITemplateRender renderer,
        IEmailTemplateRepository templateRepository) : base(sender, renderer, templateRepository)
    {
        _emailQueueRepository = emailQueueRepository;
        _orderRepository = orderRepository;
        _customerRepository = customerRepository;
        _tenantRepository = tenantRepository;
        _orderPaymentRepository = orderPaymentRepository;
        _unitOfWork = unitOfWork;
    }

    protected override async Task<EmailDataPacket> GetEmailDataAsync(OrderPaidEvent evt)
    {
        var order = await _orderRepository.GetByIdAsync(evt.OrderId)
                    ?? throw new Exception("Order not found");

        var payment = await _orderPaymentRepository.GetByOrderId(evt.OrderId)
                      ?? throw new Exception("payment not found, shouldnt happen");

        var customer = await _customerRepository.GetByIdAsync(order.CustomerId)
                       ?? throw new Exception("Customer not found");

        var tenant = await _tenantRepository.GetByIdAsync(evt.TenantId)
                     ?? throw new Exception("Tenant not found");

        var metadata = new Dictionary<string, string>
        {
            [EmailMetadataKeys.CustomerName] = customer.FirstName,
            [EmailMetadataKeys.OrderId] = order.Id.ToString(),
            [EmailMetadataKeys.AmountPaid] = order.Price.Value.ToString() + "€",
            [EmailMetadataKeys.PaymentMethod] = payment.PaymentMethod.ToString(),
            [EmailMetadataKeys.BillingAddress] = order.Address.ToString(),
            //[EmailMetadataKeys.ItemsHtml] = string.Join("", order.Items.Select(item => $"<tr><td>{item.ProductName}</td><td>{item.Quantity}</td><td>{item.UnitPrice:C}</td></tr>")),
            [EmailMetadataKeys.TenantName] = tenant.Name
        };

        return new EmailDataPacket(customer.Email.Value, tenant.Name, tenant.Email.Value, EmailTemplateNames.OrderPaid,
            metadata);
    }
}
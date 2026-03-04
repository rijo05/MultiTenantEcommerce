using MultiTenantEcommerce.Application.Commerce.Sales.Orders.Common.Mappers;
using MultiTenantEcommerce.Application.Commerce.Sales.Orders.Queries.GetByIdAdmin;
using MultiTenantEcommerce.Domain.Commerce.Sales.Orders.Interfaces;
using MultiTenantEcommerce.Shared.Application.CQRS;
using MultiTenantEcommerce.Shared.Integration.Proxies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Application.Commerce.Sales.Orders.Queries.GetOrderByPaymentReference;
public class GetOrderByPaymentReferenceQueryHandler : IQueryHandler<GetOrderByPaymentReferenceQuery, OrderDetailAdminDTO>
{
    private readonly IOrderRepository _readRepository;
    private readonly ICustomerIntegrationProxy _customerIntegrationProxy;

    public GetOrderByPaymentReferenceQueryHandler(IOrderRepository readRepository, 
        ICustomerIntegrationProxy customerIntegrationProxy)
    {
        _readRepository = readRepository;
        _customerIntegrationProxy = customerIntegrationProxy;
    }

    public async Task<OrderDetailAdminDTO> Handle(GetOrderByPaymentReferenceQuery request, CancellationToken ct)
    {
        var order = await _readRepository.GetOrderByPaymentReference(request.PaymentReference)
            ?? throw new Exception("Doesnt exist");

        var customer = await _customerIntegrationProxy.GetCustomerInfoByIdAsync(order.CustomerId);

        return order.ToAdminDetailDTO(customer.Email);
    }
}

using MediatR;
using MultiTenantEcommerce.Application.Commerce.Sales.Orders.Queries.GetByIdAdmin;
using MultiTenantEcommerce.Shared.Application.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Application.Commerce.Sales.Orders.Queries.GetOrderByPaymentReference;
public record GetOrderByPaymentReferenceQuery(
    Guid TenantId, 
    string PaymentReference) : IQuery<OrderDetailAdminDTO>;

using MultiTenantEcommerce.Shared.Application.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Application.Commerce.Sales.Orders.Queries.GetMyOrderById;
public record GetMyOrderByIdQuery(Guid CustomerId, Guid OrderId) : IQuery<OrderDetailDTO>;

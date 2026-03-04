using MultiTenantEcommerce.Shared.Application;
using MultiTenantEcommerce.Shared.Application.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Application.Commerce.Sales.Orders.Queries.GetMyOrders;
public record GetMyOrdersQuery(Guid CustomerId,
    int Page = 1,
    int PageSize = 20,
    SortOptions SortOptions = SortOptions.TimeDesc) : IQuery<PaginatedList<OrderSummaryDTO>>;

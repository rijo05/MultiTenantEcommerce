using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Application.Commerce.Sales.Orders.Queries.GetMyOrders;
public record OrderSummaryDTO(
    Guid Id,
    DateTime CreatedAt,
    string Status,
    decimal TotalPrice,
    int ItemCount);

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Application.Commerce.Sales.Orders.Queries.GetCheckoutSummary;
public record UnavailableItemDTO(
    Guid ProductId,
    string ProductName,
    int RequestedQuantity,
    int AvailableQuantity
);

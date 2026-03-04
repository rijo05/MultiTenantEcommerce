using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Application.Commerce.Sales.Orders.Queries.GetFilteredAdmin;
public record OrderSummaryAdminDTO(
    Guid Id,
    Guid CustomerId,
    string CustomerName,
    DateTime CreatedAt,
    string Status,
    decimal TotalPrice,
    string PaymentStatus);

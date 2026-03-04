using MultiTenantEcommerce.Application.Commerce.Sales.Orders.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Application.Commerce.Sales.Orders.Queries.GetByIdAdmin;
public record OrderDetailAdminDTO(
    Guid Id,
    Guid CustomerId,
    string CustomerEmail,
    DateTime CreatedAt,
    string Status,
    decimal SubTotal,
    decimal ShippingCost,
    decimal TotalPrice,
    OrderAddressDTO ShippingAddress,
    List<OrderItemDTO> Items,
    PaymentAttemptAdminDTO? Payment);

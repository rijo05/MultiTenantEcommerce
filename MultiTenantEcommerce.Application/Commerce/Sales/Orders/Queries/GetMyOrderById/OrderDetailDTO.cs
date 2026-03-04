using MultiTenantEcommerce.Application.Commerce.Sales.Orders.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Application.Commerce.Sales.Orders.Queries.GetMyOrderById;
public record OrderDetailDTO(
    Guid Id,
    DateTime CreatedAt,
    DateTime ExpiresAt, 
    string Status,
    decimal SubTotal,
    decimal ShippingCost,
    decimal TotalPrice,
    OrderAddressDTO ShippingAddress,
    List<OrderItemDTO> Items,
    PaymentSummaryCustomerDTO Payment);

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Application.Commerce.Sales.Orders.Common.DTOs;
public record OrderItemDTO(Guid ProductId, 
    string ProductName, 
    decimal UnitPrice, 
    int Quantity, 
    decimal LineTotal);

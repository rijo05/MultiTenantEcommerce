using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Application.Commerce.Sales.ShoppingCart.Common.DTOs;
public record CartItemDetailDTO(Guid ProductId, 
    string ProductName, 
    string ImageUrl,
    decimal UnitPrice, 
    int Quantity, 
    decimal TotalLinePrice);

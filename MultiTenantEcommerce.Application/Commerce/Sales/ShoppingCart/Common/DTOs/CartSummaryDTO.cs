using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Application.Commerce.Sales.ShoppingCart.Common.DTOs;
public record CartSummaryDTO(Guid CartId, 
    int TotalItems);

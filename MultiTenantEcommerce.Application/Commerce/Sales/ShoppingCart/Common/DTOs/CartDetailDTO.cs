using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Application.Commerce.Sales.ShoppingCart.Common.DTOs;
public record CartDetailDTO(Guid CartId, 
    List<CartItemDetailDTO> Items, 
    decimal SubTotal);

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Application.Commerce.Sales.Orders.Common.DTOs;
public record OrderAddressDTO(string Street, 
    string City, 
    string PostalCode, 
    string Country,
    string HouseNumber);

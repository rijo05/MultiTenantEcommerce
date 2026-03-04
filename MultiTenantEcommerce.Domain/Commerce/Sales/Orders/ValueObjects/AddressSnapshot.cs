using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Domain.Commerce.Sales.Orders.ValueObjects;
public record AddressSnapshot(
    string Street,
    string City,
    string PostalCode,
    string Country,
    string HouseNumber);

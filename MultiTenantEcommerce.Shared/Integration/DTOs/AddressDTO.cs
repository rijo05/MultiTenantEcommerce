using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Shared.Integration.DTOs;
public record AddressDTO(
    Guid Id,
    string Street,
    string City,
    string PostalCode,
    string Country,
    string HouseNumber);

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Shared.Integration.DTOs;
public record CustomerInfoDTO(
    Guid Id,
    string Name,
    string Email,
    List<AddressDTO> addresses);

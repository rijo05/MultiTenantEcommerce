using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Shared.Integration.DTOs;
public record ProductInfoDTO(
    Guid Id,
    string Name,
    decimal Price,
    string StockStatus,
    string ImageURL);

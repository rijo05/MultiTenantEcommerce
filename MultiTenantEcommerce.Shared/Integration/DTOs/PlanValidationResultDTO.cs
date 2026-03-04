using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Shared.Integration.DTOs;
public record PlanValidationResultDTO(bool IsValid, Guid? InternalPriceId, string? ErrorMessage);

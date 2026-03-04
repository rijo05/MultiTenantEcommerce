using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Domain.Platform.Tenancy.Interfaces.Services;
public interface ITenantCreationPolicy
{
    public Task EnsureUserCanCreateTenantAsync(Guid userId);
}

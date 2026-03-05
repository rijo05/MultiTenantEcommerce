using MultiTenantEcommerce.Shared.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Shared.Infrastructure.Persistence;
public class DesignTimeTenantProvider : ITenantContext
{
    public Guid TenantId => Guid.Empty;

    public void SetTenantId(Guid tenantId)
    {
        throw new NotImplementedException();
    }
}

using MultiTenantEcommerce.Shared.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Domain.Platform.Tenancy.Entities;
public class CustomDomain : TenantBase
{
    public string DomainName { get; private set; }
    public bool IsVerified { get; private set; }

    internal CustomDomain(Guid tenantId, string domainName) : base(tenantId)
    {
        DomainName = domainName.ToLower();
        IsVerified = false;
    }

    public void MarkAsVerified()
    {
        IsVerified = true;
        SetUpdatedAt();
    }
}

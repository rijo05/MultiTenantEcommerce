using MultiTenantEcommerce.Domain.Common.Entities;
using MultiTenantEcommerce.Domain.Common.Guard;

namespace MultiTenantEcommerce.Domain.Tenancy.Entities;
public class Tenant : BaseEntity
{
    public string Name { get; private set; }

    private Tenant() { }
    public Tenant(string companyName)
    {
        GuardCommon.AgainstNullOrEmpty(companyName, nameof(companyName));
        GuardCommon.AgainstMaxLength(companyName, 50 ,nameof(companyName)); 

        Name = companyName;
    }

    public void UpdateCompanyName(string companyName)
    {
        GuardCommon.AgainstNullOrEmpty(companyName, nameof(companyName));
        GuardCommon.AgainstMaxLength(companyName, 50, nameof(companyName));

        Name = companyName;
    }
}

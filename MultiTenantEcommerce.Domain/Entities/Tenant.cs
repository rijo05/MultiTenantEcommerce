using MultiTenantEcommerce.Domain.Common.Guard;

namespace MultiTenantEcommerce.Domain.Entities;
public class Tenant
{
    public Guid Id { get; private set; }
    public string CompanyName { get; private set; }
    public DateTime createdAt { get; private set; }

    public Tenant(string companyName)
    {
        GuardCommon.AgainstNullOrEmpty(companyName, nameof(companyName));
        GuardCommon.AgainstMaxLength(companyName, 50 ,nameof(companyName)); 

        Id = Guid.NewGuid();
        CompanyName = companyName;
        createdAt = DateTime.UtcNow;
    }
}

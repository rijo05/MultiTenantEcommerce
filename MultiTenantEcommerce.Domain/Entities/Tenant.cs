using MultiTenantEcommerce.Domain.Common.Guard;

namespace MultiTenantEcommerce.Domain.Entities;
public class Tenant
{
    public Guid Id { get; private set; }
    public string CompanyName { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdateAt { get; private set; }

    private Tenant() { }
    public Tenant(string companyName)
    {
        GuardCommon.AgainstNullOrEmpty(companyName, nameof(companyName));
        GuardCommon.AgainstMaxLength(companyName, 50 ,nameof(companyName)); 

        Id = Guid.NewGuid();
        CompanyName = companyName;
        CreatedAt = DateTime.UtcNow;
        UpdateAt = DateTime.UtcNow;
    }
}

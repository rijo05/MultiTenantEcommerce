using MultiTenantEcommerce.Domain.Common.Entities;
using MultiTenantEcommerce.Domain.Common.Guard;
using MultiTenantEcommerce.Domain.Tenants.Events;
using MultiTenantEcommerce.Domain.ValueObjects;

namespace MultiTenantEcommerce.Domain.Tenants.Entities;
public class Tenant : BaseEntity
{
    public string Name { get; private set; }
    public Email Email { get; private set; }

    private Tenant() { }
    public Tenant(string companyName, Email companyEmail)
    {
        GuardCommon.AgainstNullOrEmpty(companyName, nameof(companyName));
        GuardCommon.AgainstMaxLength(companyName, 50, nameof(companyName));

        Name = companyName;
        Email = companyEmail;

        AddDomainEvent(new TenantRegisteredEvent(this.Id, Email.Value));
    }

    public void UpdateTenant(string companyName)
    {
        if (!string.IsNullOrWhiteSpace(companyName))
            UpdateCompanyName(companyName);
    }

    public void UpdateCompanyName(string companyName)
    {
        GuardCommon.AgainstNullOrEmpty(companyName, nameof(companyName));
        GuardCommon.AgainstMaxLength(companyName, 50, nameof(companyName));

        Name = companyName;
        SetUpdatedAt();
    }
}

using MultiTenantEcommerce.Shared.Domain.Abstractions;

namespace MultiTenantEcommerce.Domain.Platform.Tenancy.Entities.Auth;

public class Permission : BaseEntity
{
    private Permission()
    {
    }

    public Permission(string name, string description, string area, string action)
    {
        Name = name;
        Description = description;
        Area = area;
        Action = action;
    }

    public string Name { get; private set; }
    public string Description { get; private set; }
    public string Area { get; private set; }
    public string Action { get; private set; }
}
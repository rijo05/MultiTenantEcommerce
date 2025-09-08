using MultiTenantEcommerce.Domain.Common.Entities;

namespace MultiTenantEcommerce.Domain.Users.Entities.Permissions;
public class Permission : BaseEntity
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public string Area { get; private set; }
    public string Action { get; private set; }

    private Permission() { }

    public Permission(string name, string description, string area, string action)
    {
        Name = name;
        Description = description;
        Area = area;
        Action = action;
    }
}

using MultiTenantEcommerce.Domain.Enums;

namespace MultiTenantEcommerce.Domain.ValueObjects;
public class Role
{
    public RoleType roleName { get; private set; }

    private Role() { }

    public Role(string name)
    {
        if (!Enum.TryParse<RoleType>(name, true, out var parsedRole))
            throw new Exception("Invalid Role");
        roleName = parsedRole;
    }

    public override string ToString() => roleName.ToString();
}

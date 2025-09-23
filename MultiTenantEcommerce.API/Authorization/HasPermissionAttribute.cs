namespace MultiTenantEcommerce.API.Authorization;

public class HasPermissionAttribute : Attribute
{
    public string Permission { get; }
    public HasPermissionAttribute(string permission)
    {
        Permission = permission;
    }
}

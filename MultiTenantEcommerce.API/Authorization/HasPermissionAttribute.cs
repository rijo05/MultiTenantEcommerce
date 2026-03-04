namespace MultiTenantEcommerce.API.Authorization;

public class HasPermissionAttribute : Attribute
{
    public HasPermissionAttribute(string permission)
    {
        Permission = permission;
    }

    public string Permission { get; }
}
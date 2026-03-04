using System.Security.Claims;

namespace MultiTenantEcommerce.API.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal claims)
    {
        return Guid.Parse(claims.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    }

    public static Guid GetTenantId(this ClaimsPrincipal claims)
    {
        return Guid.Parse(claims.FindFirst("tenantId")!.Value);
    }
}
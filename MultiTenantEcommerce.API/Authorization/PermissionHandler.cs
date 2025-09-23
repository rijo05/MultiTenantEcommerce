using Microsoft.AspNetCore.Authorization;

namespace MultiTenantEcommerce.API.Authorization;

public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        if (context.Resource is HttpContext httpContext)
        {
            var endpoint = httpContext.GetEndpoint();
            var permissionAttribute = endpoint?.Metadata.GetMetadata<HasPermissionAttribute>();
            if (permissionAttribute == null)
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            var permissions = context.User.FindAll("Permission").Select(c => c.Value);
            if (permissions.Contains(permissionAttribute.Permission))
                context.Succeed(requirement);
        }
        return Task.CompletedTask;
    }
}

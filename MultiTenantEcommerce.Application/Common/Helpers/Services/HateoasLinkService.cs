namespace MultiTenantEcommerce.Application.Common.Helpers.Services;

public class HateoasLinkService
{
    //private readonly LinkGenerator _linkGenerator;
    //private readonly IHttpContextAccessor _httpContextAccessor;

    //public HateoasLinkService(LinkGenerator linkGenerator, IHttpContextAccessor httpContextAccessor)
    //{
    //    _linkGenerator = linkGenerator;
    //    _httpContextAccessor = httpContextAccessor;
    //}

    //public Dictionary<string, object> GenerateLinksCRUD(Guid id, string controllerName, string getAction, string updateAction, string deleteAction)
    //{
    //    var httpContext = _httpContextAccessor.HttpContext;

    //    if (httpContext is null)
    //        throw new InvalidOperationException("HttpContext is not available.");

    //    return new Dictionary<string, object>
    //    {
    //        { "self", new { href = _linkGenerator.GetPathByAction(httpContext, getAction, controllerName, new { id }), method = "GET" } },
    //        { "update", new { href = _linkGenerator.GetPathByAction(httpContext, updateAction, controllerName, new { id }), method = "PATCH" } },
    //        { "delete", new { href = _linkGenerator.GetPathByAction(httpContext, deleteAction, controllerName, new { id }), method = "DELETE" } }
    //    };
    //}
}

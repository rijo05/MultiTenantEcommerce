using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Options;
using MultiTenantEcommerce.Domain.Interfaces;
using MultiTenantEcommerce.Infrastructure.Configurations;
using MultiTenantEcommerce.Infrastructure.Context;

namespace MultiTenantEcommerce.API.Middleware;

public class TenantMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ITenantRepository _tenantRepository ;

    public TenantMiddleware(RequestDelegate request, ITenantRepository tenantRepository)
    {
        _next = request;
        _tenantRepository = tenantRepository;
    }

    public async Task InvokeAsync(HttpContext context, TenantContext tenantContext, IOptions<AppSettings> appSettings)
    {
        var companyName = context.Request.Host.Value.Replace("."+ appSettings.Value.PlatformUrl,"");

        var tenant = await _tenantRepository.GetByCompanyName(companyName);

        if (tenant == null)
            throw new Exception("That store doesnt exist");

        tenantContext.TenantId = tenant.Id;

        await _next(context);
    }
}

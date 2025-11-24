using Microsoft.Extensions.Options;
using MultiTenantEcommerce.Application.Common.Interfaces.Persistence;
using MultiTenantEcommerce.Infrastructure.Persistence.Configurations;

namespace MultiTenantEcommerce.API.Middleware;

public class TenantMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IServiceScopeFactory _scopeFactory;

    public TenantMiddleware(RequestDelegate next, IServiceScopeFactory scopeFactory)
    {
        _next = next;
        _scopeFactory = scopeFactory;
    }

    public async Task InvokeAsync(HttpContext context, ITenantContext tenantContext, IOptions<AppSettings> appSettings)
    {
        //var companyName = context.Request.Host.Value.Replace("." + appSettings.Value.PlatformUrl, "");

        //var host = context.Request.Host.Host; // devolve "tenant1.localhost"
        //var companyName = host.Split('.')[0]; // devolve "tenant1"

        //using (var scope = _scopeFactory.CreateScope())
        //{
        //    var tenantRepository = scope.ServiceProvider.GetRequiredService<ITenantRepository>();

        //    var tenant = await tenantRepository.GetByCompanyName(companyName);

        //    if (tenant == null)
        //        throw new Exception($"{companyName} doesn't exist");

        //    tenantContext.SetTenantId(tenant.Id);
        //}

        tenantContext.SetTenantId(Guid.Parse("6b936f20-b702-49d8-a0b0-c3d90c2db69a"));

        //context.Items["TenantName"] = companyName;

        await _next(context);
    }
}

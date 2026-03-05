using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MultiTenantEcommerce.Application.Commerce.Catalog.Interfaces;
using MultiTenantEcommerce.Domain.Commerce.Catalog.Interfaces;
using MultiTenantEcommerce.Infrastructure.Commerce.Catalog.Persistence.Repositories;
using MultiTenantEcommerce.Infrastructure.Messaging;
using MultiTenantEcommerce.Infrastructure.Persistence.Catalog.Context;
using MultiTenantEcommerce.Infrastructure.Persistence.Catalog.Repositories;
using MultiTenantEcommerce.Infrastructure.Persistence.Repositories;
using MultiTenantEcommerce.Shared.Application.Events;

namespace MultiTenantEcommerce.Infrastructure.Commerce.Catalog;
public static class CatalogModule
{
    public static IServiceCollection AddCatalogModules(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<CatalogDbContext>(options =>
            options.UseNpgsql(config.GetConnectionString("DefaultConnection")));

        services.AddScoped<ICatalogUnitOfWork, CatalogUnitOfWork>();

        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();

        return services;
    }
}

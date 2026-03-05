using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using MultiTenantEcommerce.Infrastructure.Persistence.Catalog.Context;
using MultiTenantEcommerce.Infrastructure.Persistence.Context;
using MultiTenantEcommerce.Infrastructure.Shared.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Infrastructure.Commerce.Catalog.Persistence.Context;
public class CatalogDbContextFactory : IDesignTimeDbContextFactory<CatalogDbContext>
{
    public CatalogDbContext CreateDbContext(string[] args)
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile(@"C:\...\appsettings.json")
            .Build();

        var options = new DbContextOptionsBuilder<CatalogDbContext>()
            .UseNpgsql(config.GetConnectionString("DefaultConnection"))
            .Options;

        return new CatalogDbContext(options, new DesignTimeTenantProvider());
    }
}

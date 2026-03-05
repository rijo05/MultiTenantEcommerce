using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using MultiTenantEcommerce.Infrastructure.Shared.Persistence;

namespace MultiTenantEcommerce.Infrastructure.Commerce.Shipping.Persistence.Context;

public class ShippingDbContextFactory : IDesignTimeDbContextFactory<ShippingDbContext>
{
    public ShippingDbContext CreateDbContext(string[] args)
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile(@"C:\...\appsettings.json")
            .Build();

        var options = new DbContextOptionsBuilder<ShippingDbContext>()
            .UseNpgsql(config.GetConnectionString("DefaultConnection"))
            .Options;

        return new ShippingDbContext(options, new DesignTimeTenantProvider());
    }
}

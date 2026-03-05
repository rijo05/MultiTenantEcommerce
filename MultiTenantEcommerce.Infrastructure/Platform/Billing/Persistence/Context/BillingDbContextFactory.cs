using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using MultiTenantEcommerce.Infrastructure.Shared.Persistence;

namespace MultiTenantEcommerce.Infrastructure.Platform.Billing.Persistence.Context;

public class BillingDbContextFactory : IDesignTimeDbContextFactory<BillingDbContext>
{
    public BillingDbContext CreateDbContext(string[] args)
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile(@"C:\...\appsettings.json")
            .Build();

        var options = new DbContextOptionsBuilder<BillingDbContext>()
            .UseNpgsql(config.GetConnectionString("DefaultConnection"))
            .Options;

        return new BillingDbContext(options, new DesignTimeTenantProvider());
    }
}

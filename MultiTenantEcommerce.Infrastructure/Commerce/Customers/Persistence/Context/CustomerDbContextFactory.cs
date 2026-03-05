using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using MultiTenantEcommerce.Infrastructure.Shared.Persistence;

namespace MultiTenantEcommerce.Infrastructure.Commerce.Customers.Persistence.Context;

public class CustomerDbContextFactory : IDesignTimeDbContextFactory<CustomerDbContext>
{
    public CustomerDbContext CreateDbContext(string[] args)
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile(@"C:\...\appsettings.json")
            .Build();

        var options = new DbContextOptionsBuilder<CustomerDbContext>()
            .UseNpgsql(config.GetConnectionString("DefaultConnection"))
            .Options;

        return new CustomerDbContext(options, new DesignTimeTenantProvider());
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using MultiTenantEcommerce.Infrastructure.Shared.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Infrastructure.Platform.Identity.Persistence.Context;
public class IdentityDbContextFactory : IDesignTimeDbContextFactory<IdentityDbContext>
{
    public IdentityDbContext CreateDbContext(string[] args)
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile(@"C:\...\appsettings.json")
            .Build();

        var options = new DbContextOptionsBuilder<IdentityDbContext>()
            .UseNpgsql(config.GetConnectionString("DefaultConnection"))
            .Options;

        return new IdentityDbContext(options, new DesignTimeTenantProvider());
    }
}

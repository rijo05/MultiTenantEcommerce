using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using MultiTenantEcommerce.Domain.Catalog.Entities;
using MultiTenantEcommerce.Domain.Common.Entities;
using MultiTenantEcommerce.Domain.Common.Events;
using MultiTenantEcommerce.Domain.Inventory.Entities;
using MultiTenantEcommerce.Domain.Sales.Entities;
using MultiTenantEcommerce.Domain.Tenancy.Entities;
using MultiTenantEcommerce.Domain.Users.Entities;
using MultiTenantEcommerce.Infrastructure.Persistence.Configurations;

namespace MultiTenantEcommerce.Infrastructure.Persistence.Context;
public class AppDbContext : DbContext
{
    private readonly TenantContext _tenantContext;
    public AppDbContext(DbContextOptions<AppDbContext> options, TenantContext tenantContext) : base(options)
    {
        _tenantContext = tenantContext;
    }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        _tenantContext = null;
    }

    public virtual DbSet<Category> Categories { get; set; }
    public virtual DbSet<Customer> Customers { get; set; }
    public virtual DbSet<Employee> Employees { get; set; }
    public virtual DbSet<Order> Orders { get; set; }
    public virtual DbSet<OrderItem> OrderItems { get; set; }
    public virtual DbSet<Product> Products { get; set; }
    public virtual DbSet<Stock> Stocks { get; set; }
    public virtual DbSet<StockMovement> StockMovements { get; set; }
    public virtual DbSet<Tenant> Tenants { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new CategoryConfiguration(_tenantContext));
        modelBuilder.ApplyConfiguration(new CustomerConfiguration(_tenantContext));
        modelBuilder.ApplyConfiguration(new EmployeeConfiguration(_tenantContext));
        modelBuilder.ApplyConfiguration(new OrderConfiguration(_tenantContext));
        modelBuilder.ApplyConfiguration(new OrderItemConfiguration(_tenantContext));
        modelBuilder.ApplyConfiguration(new ProductConfiguration(_tenantContext));
        modelBuilder.ApplyConfiguration(new StockConfiguration(_tenantContext));
        modelBuilder.ApplyConfiguration(new StockMovementConfiguration(_tenantContext));
        modelBuilder.ApplyConfiguration(new TenantConfiguration());
    }

    public override int SaveChanges()
    {
        var tenantId = _tenantContext.TenantId;

        if (tenantId == Guid.Empty)
        {
            throw new InvalidOperationException("TenantId is not set in the TenantContext.");
        }

        foreach (var entry in ChangeTracker.Entries<TenantBase>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.SetTenantId(tenantId);
                entry.Entity.SetUpdatedAt();
                entry.Entity.SetUpdatedAt();
            }
            else if (entry.State == EntityState.Modified)
                entry.Entity.SetUpdatedAt();
        }

        return base.SaveChanges();
    }

    #region EVENTS
    public List<IDomainEvent> GetAllDomainEvents()
    {
        return ChangeTracker
            .Entries<IHasDomainEvents>()
            .SelectMany(e => e.Entity.DomainEvents)
            .ToList();
    }

    public void ClearAllDomainEvents()
    {
        var domainEntities = ChangeTracker
            .Entries<IHasDomainEvents>()
            .Where(x => x.Entity.DomainEvents.Any())
            .Select(x => x.Entity);

        foreach (var entity in domainEntities)
            entity.ClearDomainEvents();
    }

    #endregion
}

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(@"D:\MultiTenantEcommerce\MultiTenantEcommerce\MultiTenantEcommerce.API\appsettings.json", optional: false, reloadOnChange: true);

        var configuration = builder.Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        optionsBuilder.UseNpgsql(connectionString);

        return new AppDbContext(optionsBuilder.Options);
    }
}


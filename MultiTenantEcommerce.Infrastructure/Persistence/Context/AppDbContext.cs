using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using MultiTenantEcommerce.Application.Common.Interfaces.Persistence;
using MultiTenantEcommerce.Domain.Catalog.Entities;
using MultiTenantEcommerce.Domain.Common.Entities;
using MultiTenantEcommerce.Domain.Common.Events;
using MultiTenantEcommerce.Domain.Inventory.Entities;
using MultiTenantEcommerce.Domain.Payment.Entities;
using MultiTenantEcommerce.Domain.Sales.Orders.Entities;
using MultiTenantEcommerce.Domain.Sales.ShoppingCart.Entities;
using MultiTenantEcommerce.Domain.Shipping.Entities;
using MultiTenantEcommerce.Domain.Templates.Entities;
using MultiTenantEcommerce.Domain.Tenants.Entities;
using MultiTenantEcommerce.Domain.Users.Entities;
using MultiTenantEcommerce.Domain.Users.Entities.Permissions;
using MultiTenantEcommerce.Infrastructure.EmailService;
using MultiTenantEcommerce.Infrastructure.Events;
using MultiTenantEcommerce.Infrastructure.Outbox;

namespace MultiTenantEcommerce.Infrastructure.Persistence.Context;
public class AppDbContext : DbContext
{
    private readonly ITenantContext _tenantContext;
    public AppDbContext(DbContextOptions<AppDbContext> options,
        ITenantContext tenantContext)
        : base(options)
    {
        _tenantContext = tenantContext;
    }

    public virtual DbSet<Category> Categories { get; set; }
    public virtual DbSet<Product> Products { get; set; }
    public virtual DbSet<ProductImages> ProductImages { get; set; }
    public virtual DbSet<Customer> Customers { get; set; }
    public virtual DbSet<Employee> Employees { get; set; }
    public virtual DbSet<Stock> Stocks { get; set; }
    public virtual DbSet<StockMovement> StockMovements { get; set; }
    public virtual DbSet<Shipment> Shipments { get; set; }
    public virtual DbSet<Order> Orders { get; set; }
    public virtual DbSet<OrderItem> OrderItems { get; set; }
    public virtual DbSet<Cart> Carts { get; set; }
    public virtual DbSet<CartItem> CartItems { get; set; }
    public virtual DbSet<OrderPayment> OrderPayments { get; set; }
    public virtual DbSet<Tenant> Tenants { get; set; }
    public virtual DbSet<Role> Roles { get; set; }
    public virtual DbSet<Permission> Permissions { get; set; }
    public virtual DbSet<EmployeeRole> EmployeeRoles { get; set; }
    public virtual DbSet<OutboxEvent> OutboxEvents { get; set; }
    public virtual DbSet<ProcessedEvent> ProcessedEvents { get; set; }
    public virtual DbSet<EmailQueue> EmailQueue { get; set; }
    public virtual DbSet<EmailTemplate> EmailTemplates { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);


        modelBuilder.Entity<Category>().HasQueryFilter(x => x.TenantId == _tenantContext.TenantId);
        modelBuilder.Entity<Product>().HasQueryFilter(x => x.TenantId == _tenantContext.TenantId);
        modelBuilder.Entity<ProductImages>().HasQueryFilter(x => x.TenantId == _tenantContext.TenantId);

        modelBuilder.Entity<Customer>().HasQueryFilter(x => x.TenantId == _tenantContext.TenantId);
        modelBuilder.Entity<Employee>().HasQueryFilter(x => x.TenantId == _tenantContext.TenantId);

        modelBuilder.Entity<Shipment>().HasQueryFilter(x => x.TenantId == _tenantContext.TenantId);

        modelBuilder.Entity<Order>().HasQueryFilter(x => x.TenantId == _tenantContext.TenantId);
        modelBuilder.Entity<OrderItem>().HasQueryFilter(x => x.TenantId == _tenantContext.TenantId);
        modelBuilder.Entity<Cart>().HasQueryFilter(x => x.TenantId == _tenantContext.TenantId);
        modelBuilder.Entity<CartItem>().HasQueryFilter(x => x.TenantId == _tenantContext.TenantId);

        modelBuilder.Entity<Stock>().HasQueryFilter(x => x.TenantId == _tenantContext.TenantId);
        modelBuilder.Entity<StockMovement>().HasQueryFilter(x => x.TenantId == _tenantContext.TenantId);

        modelBuilder.Entity<OrderPayment>().HasQueryFilter(x => x.TenantId == _tenantContext.TenantId);

        modelBuilder.Entity<Role>().HasQueryFilter(x => x.TenantId == _tenantContext.TenantId);
        modelBuilder.Entity<EmployeeRole>().HasQueryFilter(x => x.TenantId == _tenantContext.TenantId);

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
            .AddJsonFile(@"C:\MultiTenantEcommerce\MultiTenantEcommerce\MultiTenantEcommerce.API\appsettings.json", optional: false, reloadOnChange: true);

        var configuration = builder.Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        optionsBuilder.UseNpgsql(connectionString);

        return new AppDbContext(optionsBuilder.Options, new DesignTimeTenantProvider());
    }
}

public class DesignTimeTenantProvider : ITenantContext
{
    public Guid TenantId { get; set; } = Guid.Empty;

    public void SetTenantId(Guid tenantId)
    {
        throw new NotImplementedException();
    }
}


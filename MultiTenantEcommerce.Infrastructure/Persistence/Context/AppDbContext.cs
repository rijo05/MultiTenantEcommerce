using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using MultiTenantEcommerce.Domain.Commerce.Catalog.Entities;
using MultiTenantEcommerce.Domain.Commerce.Customers.Entities;
using MultiTenantEcommerce.Domain.Commerce.Inventory.Entities;
using MultiTenantEcommerce.Domain.Commerce.Sales.Orders.Entities;
using MultiTenantEcommerce.Domain.Commerce.Sales.ShoppingCart.Entities;
using MultiTenantEcommerce.Domain.Commerce.Shipping.Entities;
using MultiTenantEcommerce.Domain.Notifications.Entities;
using MultiTenantEcommerce.Domain.Platform.Billing.Entities;
using MultiTenantEcommerce.Domain.Platform.Identity.Entities;
using MultiTenantEcommerce.Domain.Platform.Tenancy.Entities;
using MultiTenantEcommerce.Domain.Platform.Tenancy.Entities.Auth;
using MultiTenantEcommerce.Infrastructure.Events;
using MultiTenantEcommerce.Infrastructure.Outbox;
using MultiTenantEcommerce.Shared.Domain.Abstractions;
using MultiTenantEcommerce.Shared.Domain.Events;
using MultiTenantEcommerce.Shared.Infrastructure.Persistence;

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
    public virtual DbSet<TenantMember> TenantMembers { get; set; }
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Stock> Stocks { get; set; }
    public virtual DbSet<StockMovement> StockMovements { get; set; }
    public virtual DbSet<Shipment> Shipments { get; set; }
    public virtual DbSet<ShippingProviderConfig> ShippingProviderConfigs { get; set; }
    public virtual DbSet<Order> Orders { get; set; }
    public virtual DbSet<OrderItem> OrderItems { get; set; }
    public virtual DbSet<Cart> Carts { get; set; }
    public virtual DbSet<CartItem> CartItems { get; set; }
    public virtual DbSet<OrderPayment> OrderPayments { get; set; }
    public virtual DbSet<Tenant> Tenants { get; set; }
    public virtual DbSet<TenantSubscription> TenantSubscriptions { get; set; }
    public virtual DbSet<SubscriptionPlan> SubscriptionPlans { get; set; }
    public virtual DbSet<SubscriptionPlanPrice> SubscriptionPlanPrices { get; set; }
    public virtual DbSet<Role> Roles { get; set; }
    public virtual DbSet<RolePermission> RolePermissions { get; set; }
    public virtual DbSet<Permission> Permissions { get; set; }
    public virtual DbSet<TenantMemberRole> TenantMemberRoles { get; set; }
    public virtual DbSet<TenantInvitation> TenantInvitations { get; set; }
    public virtual DbSet<OutboxEvent> OutboxEvents { get; set; }
    public virtual DbSet<ProcessedEvent> ProcessedEvents { get; set; }
    public virtual DbSet<TenantNotificationProfile> NotificationProfiles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);


        modelBuilder.Entity<Category>().HasQueryFilter(x => x.TenantId == _tenantContext.TenantId);
        modelBuilder.Entity<Product>().HasQueryFilter(x => x.TenantId == _tenantContext.TenantId);
        modelBuilder.Entity<ProductImages>().HasQueryFilter(x => x.TenantId == _tenantContext.TenantId);

        modelBuilder.Entity<Customer>().HasQueryFilter(x => x.TenantId == _tenantContext.TenantId);
        modelBuilder.Entity<TenantMember>().HasQueryFilter(x => x.TenantId == _tenantContext.TenantId);

        modelBuilder.Entity<Shipment>().HasQueryFilter(x => x.TenantId == _tenantContext.TenantId);
        modelBuilder.Entity<ShippingProviderConfig>().HasQueryFilter(x => x.TenantId == _tenantContext.TenantId);

        modelBuilder.Entity<Order>().HasQueryFilter(x => x.TenantId == _tenantContext.TenantId);
        modelBuilder.Entity<OrderItem>().HasQueryFilter(x => x.TenantId == _tenantContext.TenantId);
        modelBuilder.Entity<Cart>().HasQueryFilter(x => x.TenantId == _tenantContext.TenantId);
        modelBuilder.Entity<CartItem>().HasQueryFilter(x => x.TenantId == _tenantContext.TenantId);

        modelBuilder.Entity<Stock>().HasQueryFilter(x => x.TenantId == _tenantContext.TenantId);
        modelBuilder.Entity<StockMovement>().HasQueryFilter(x => x.TenantId == _tenantContext.TenantId);

        modelBuilder.Entity<OrderPayment>().HasQueryFilter(x => x.TenantId == _tenantContext.TenantId);

        modelBuilder.Entity<Role>().HasQueryFilter(x => x.TenantId == _tenantContext.TenantId);
        modelBuilder.Entity<TenantMemberRole>().HasQueryFilter(x => x.TenantId == _tenantContext.TenantId);
        modelBuilder.Entity<TenantInvitation>().HasQueryFilter(x => x.TenantId == _tenantContext.TenantId);
        modelBuilder.Entity<RolePermission>().HasQueryFilter(x => x.TenantId == _tenantContext.TenantId);
    }

    public override int SaveChanges()
    {
        var tenantId = _tenantContext.TenantId;

        if (tenantId == Guid.Empty) throw new InvalidOperationException("TenantId is not set in the TenantContext.");

        foreach (var entry in ChangeTracker.Entries<TenantBase>())
            if (entry.State == EntityState.Added)
            {
                entry.Entity.SetTenantId(tenantId);
                entry.Entity.SetUpdatedAt();
                entry.Entity.SetUpdatedAt();
            }
            else if (entry.State == EntityState.Modified)
            {
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
            .AddJsonFile(@"C:\MultiTenantEcommerce\MultiTenantEcommerce\MultiTenantEcommerce.API\appsettings.json",
                false, true);

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
using Microsoft.EntityFrameworkCore;
using MultiTenantEcommerce.Domain.Common;
using MultiTenantEcommerce.Domain.Entities;
using MultiTenantEcommerce.Domain.ValueObjects;

namespace MultiTenantEcommerce.Infrastructure;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
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


        #region PRIMARY KEYS

        modelBuilder.Entity<Category>()
            .HasKey(x => new { x.TenantId, x.Id });

        modelBuilder.Entity<Customer>()
            .HasKey(x => new { x.TenantId, x.Id });

        modelBuilder.Entity<Employee>()
            .HasKey(x => new { x.TenantId, x.Id });

        modelBuilder.Entity<Order>()
            .HasKey(x => new { x.TenantId, x.Id });

        modelBuilder.Entity<OrderItem>()
            .HasKey(x => new { x.TenantId, x.OrderId, x.ProductId });

        modelBuilder.Entity<Product>()
            .HasKey(x => new { x.TenantId, x.Id });

        modelBuilder.Entity<Stock>()
            .HasKey(x => new { x.TenantId, x.Id });

        modelBuilder.Entity<StockMovement>()
            .HasKey(x => new { x.TenantId, x.Id });

        modelBuilder.Entity<Tenant>()
            .HasKey(x => x.Id);

        #endregion


        #region FOREIGN KEYS

        modelBuilder.Entity<Category>()
            .HasOne<Tenant>()
            .WithMany()
            .HasForeignKey(x => x.TenantId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Customer>()
            .HasOne<Tenant>()
            .WithMany()
            .HasForeignKey(x => x.TenantId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Employee>()
            .HasOne<Tenant>()
            .WithMany()
            .HasForeignKey(x => x.TenantId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Order>()
            .HasOne<Tenant>()
            .WithMany()
            .HasForeignKey(x => x.TenantId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Order>()
            .HasOne<Customer>()
            .WithMany()
            .HasForeignKey(x => new { x.TenantId, x.CustomerId })
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<OrderItem>()
            .HasOne<Order>()
            .WithMany()
            .HasForeignKey(x => new { x.TenantId, x.OrderId })
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<OrderItem>()
            .HasOne<Product>()
            .WithMany()
            .HasForeignKey(x => new { x.TenantId, x.ProductId })
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Product>()
            .HasOne<Tenant>()
            .WithMany()
            .HasForeignKey(x => x.TenantId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Product>()
            .HasOne<Category>()
            .WithMany()
            .HasForeignKey(x => new { x.TenantId, x.CategoryId })
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Stock>()    
            .HasOne<Product>()
            .WithOne()
            .HasForeignKey<Stock>(s => new { s.TenantId, s.ProductId })
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<StockMovement>()
            .HasOne<Product>()
            .WithMany()
            .HasForeignKey(x => new { x.TenantId, x.ProductId })
            .OnDelete(DeleteBehavior.Restrict);

        #endregion


        #region VALUE OBJECTS

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.OwnsOne(u => u.Email, email =>
            {
                email.Property(e => e.Value)
                    .HasColumnName("Email")
                    .IsRequired();
            });
            entity.OwnsOne(u => u.Password, password =>
            {
                password.Property(e => e.Value)
                    .HasColumnName("Password")
                    .IsRequired();
            });
            entity.OwnsOne(u => u.Role, role =>
            {
                role.Property(r => r.roleName)
                    .HasConversion<string>()
                    .HasColumnName("Role")
                    .IsRequired();
            });
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.OwnsOne(u => u.Email, email =>
            {
                email.Property(e => e.Value)
                    .HasColumnName("Email")
                    .IsRequired();
            });
            entity.OwnsOne(u => u.Password, password =>
            {
                password.Property(e => e.Value)
                    .HasColumnName("Password")
                    .IsRequired();
            });
            entity.OwnsOne(u => u.PhoneNumber, phoneNumber =>
            {
                phoneNumber.Property(e => e.ToString())
                    .HasColumnName("PhoneNumber")
                    .IsRequired();
            });
            entity.OwnsOne(o => o.Address, address =>
            {
                address.Property(a => a.Street).HasColumnName("Street").IsRequired();
                address.Property(a => a.HouseNumber).HasColumnName("HouseNumber").IsRequired();
                address.Property(a => a.City).HasColumnName("City").IsRequired();
                address.Property(a => a.PostalCode).HasColumnName("PostalCode").IsRequired();
                address.Property(a => a.Country).HasColumnName("Country").IsRequired();
            });
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.OwnsOne(o => o.Address, address =>
            {
                address.Property(a => a.Street).HasColumnName("Street").IsRequired();
                address.Property(a => a.HouseNumber).HasColumnName("HouseNumber").IsRequired();
                address.Property(a => a.City).HasColumnName("City").IsRequired();
                address.Property(a => a.PostalCode).HasColumnName("PostalCode").IsRequired();
                address.Property(a => a.Country).HasColumnName("Country").IsRequired();
            });

            entity.Property(o => o.OrderStatus)
                .HasConversion<string>()
                .IsRequired();
            entity.Property(o => o.PaymentMethod)
                .HasConversion<string>()
                .IsRequired();
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.OwnsOne(u => u.UnitPrice, price =>
            {
                price.Property(e => e.Value)
                    .HasColumnName("UnitPrice")
                    .IsRequired();
            });
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.OwnsOne(u => u.Price, price =>
            {
                price.Property(e => e.Value)
                    .HasColumnName("Price")
                    .IsRequired();
            });
        });

        modelBuilder.Entity<StockMovement>(entity =>
        {
            entity.Property(o => o.Reason)
                .HasConversion<string>()
                .IsRequired();
        });

        #endregion


        #region INDEXES

        modelBuilder.Entity<Category>()
            .HasIndex(c => c.TenantId)
            .HasDatabaseName("IX_Category_TenantId");

        modelBuilder.Entity<Customer>()
            .HasIndex(c => c.TenantId)
            .HasDatabaseName("IX_Customer_TenantId");

        modelBuilder.Entity<Employee>()
            .HasIndex(e => e.TenantId)
            .HasDatabaseName("IX_Employee_TenantId");

        modelBuilder.Entity<Order>()
            .HasIndex(o => o.TenantId)
            .HasDatabaseName("IX_Order_TenantId");

        modelBuilder.Entity<OrderItem>()
            .HasIndex(oi => oi.TenantId)
            .HasDatabaseName("IX_OrderItem_TenantId");

        modelBuilder.Entity<Product>()
            .HasIndex(p => p.TenantId)
            .HasDatabaseName("IX_Product_TenantId");

        modelBuilder.Entity<Stock>()
            .HasIndex(s => s.TenantId)
            .HasDatabaseName("IX_Stock_TenantId");

        modelBuilder.Entity<StockMovement>()
            .HasIndex(sm => sm.TenantId)
            .HasDatabaseName("IX_StockMovement_TenantId");



        modelBuilder.Entity<Order>()
            .HasIndex(o => new { o.TenantId, o.CustomerId })
            .HasDatabaseName("IX_Order_TenantId_CustomerId");

        modelBuilder.Entity<OrderItem>()
            .HasIndex(oi => new { oi.TenantId, oi.OrderId })
            .HasDatabaseName("IX_OrderItem_TenantId_OrderId");

        modelBuilder.Entity<OrderItem>()
            .HasIndex(oi => new { oi.TenantId, oi.ProductId })
            .HasDatabaseName("IX_OrderItem_TenantId_ProductId");

        modelBuilder.Entity<Stock>()
            .HasIndex(s => new { s.TenantId, s.ProductId })
            .HasDatabaseName("IX_Stock_TenantId_ProductId");

        modelBuilder.Entity<StockMovement>()
            .HasIndex(sm => new { sm.TenantId, sm.ProductId })
            .HasDatabaseName("IX_StockMovement_TenantId_ProductId");

        modelBuilder.Entity<Product>()
            .HasIndex(p => new { p.TenantId, p.CategoryId })
            .HasDatabaseName("IX_Product_TenantId_CategoryId");

        modelBuilder.Entity<Product>()
            .HasIndex(p => new { p.TenantId, p.Name })
            .HasDatabaseName("IX_Product_TenantId_ProductName");

        modelBuilder.Entity<Customer>()
            .HasIndex(c => new { c.TenantId, c.Email })
            .HasDatabaseName("IX_Customer_TenantId_Email");

        modelBuilder.Entity<Customer>()
            .HasIndex(c => new { c.TenantId, c.PhoneNumber })
            .HasDatabaseName("IX_Customer_TenantId_PhoneNumber");

        modelBuilder.Entity<Employee>()
            .HasIndex(e => new { e.TenantId, e.Email })
            .HasDatabaseName("IX_Employee_TenantId_Email");

        modelBuilder.Entity<Employee>()
            .HasIndex(e => new { e.TenantId, e.Role })
            .HasDatabaseName("IX_Employee_TenantId_Role");

        #endregion
    }

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
}

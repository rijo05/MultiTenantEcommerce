using FluentValidation;
using Hangfire;
using Hangfire.PostgreSql;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MultiTenantEcommerce.API.Authorization;
using MultiTenantEcommerce.API.Middleware;
using MultiTenantEcommerce.Application;
using MultiTenantEcommerce.Application.Auth.Commands.CreateCustomer;
using MultiTenantEcommerce.Application.Auth.Commands.CreateEmployee;
using MultiTenantEcommerce.Application.Auth.Commands.CreateTenant;
using MultiTenantEcommerce.Application.Auth.Services;
using MultiTenantEcommerce.Application.Catalog.Categories.Commands.Create;
using MultiTenantEcommerce.Application.Catalog.Categories.Commands.Update;
using MultiTenantEcommerce.Application.Catalog.Categories.Mappers;
using MultiTenantEcommerce.Application.Catalog.Categories.Validators;
using MultiTenantEcommerce.Application.Catalog.Products.Commands.Create;
using MultiTenantEcommerce.Application.Catalog.Products.Commands.Update;
using MultiTenantEcommerce.Application.Catalog.Products.Mappers;
using MultiTenantEcommerce.Application.Catalog.Products.Validators;
using MultiTenantEcommerce.Application.Common.DTOs.Address;
using MultiTenantEcommerce.Application.Common.Helpers.Mappers;
using MultiTenantEcommerce.Application.Common.Helpers.Services;
using MultiTenantEcommerce.Application.Common.Helpers.Validators.AddressValidator;
using MultiTenantEcommerce.Application.Common.Interfaces.Persistence;
using MultiTenantEcommerce.Application.Common.Interfaces.Services;
using MultiTenantEcommerce.Application.Inventory.EventHandlers;
using MultiTenantEcommerce.Application.Inventory.Mappers;
using MultiTenantEcommerce.Application.Inventory.Services;
using MultiTenantEcommerce.Application.Payment.OrderPayment.Interfaces;
using MultiTenantEcommerce.Application.Payment.OrderPayment.Mappers;
using MultiTenantEcommerce.Application.Sales.Orders.EventHandlers;
using MultiTenantEcommerce.Application.Sales.Orders.Mappers;
using MultiTenantEcommerce.Application.Sales.ShoppingCart.Mappers;
using MultiTenantEcommerce.Application.Shipping.EventHandlers;
using MultiTenantEcommerce.Application.Shipping.Interfaces;
using MultiTenantEcommerce.Application.Shipping.Services;
using MultiTenantEcommerce.Application.Tenants.Commands.Tenant.Update;
using MultiTenantEcommerce.Application.Tenants.Mappers;
using MultiTenantEcommerce.Application.Tenants.Validators.TenantValidator;
using MultiTenantEcommerce.Application.Users.Customers.Commands.Update;
using MultiTenantEcommerce.Application.Users.Customers.EventHandlers;
using MultiTenantEcommerce.Application.Users.Customers.Mappers;
using MultiTenantEcommerce.Application.Users.Customers.Validators;
using MultiTenantEcommerce.Application.Users.Employees.Commands.Update;
using MultiTenantEcommerce.Application.Users.Employees.EventHandlers.cs;
using MultiTenantEcommerce.Application.Users.Employees.Mappers;
using MultiTenantEcommerce.Application.Users.Employees.Validators;
using MultiTenantEcommerce.Application.Users.Permissions.Mappers;
using MultiTenantEcommerce.Domain.Catalog.Interfaces;
using MultiTenantEcommerce.Domain.Common.Interfaces;
using MultiTenantEcommerce.Domain.Enums;
using MultiTenantEcommerce.Domain.Inventory.Events;
using MultiTenantEcommerce.Domain.Inventory.Interfaces;
using MultiTenantEcommerce.Domain.Payment.Interfaces;
using MultiTenantEcommerce.Domain.Sales.Orders.Events;
using MultiTenantEcommerce.Domain.Sales.Orders.Interfaces;
using MultiTenantEcommerce.Domain.Sales.ShoppingCart.Interfaces;
using MultiTenantEcommerce.Domain.Shipping.Events;
using MultiTenantEcommerce.Domain.Templates.Entities;
using MultiTenantEcommerce.Domain.Tenants.Interfaces;
using MultiTenantEcommerce.Domain.Users.Events;
using MultiTenantEcommerce.Domain.Users.Interfaces;
using MultiTenantEcommerce.Domain.Users.Interfaces.Permissions;
using MultiTenantEcommerce.Infrastructure.AddressValidator;
using MultiTenantEcommerce.Infrastructure.EmailService;
using MultiTenantEcommerce.Infrastructure.Events;
using MultiTenantEcommerce.Infrastructure.Messaging;
using MultiTenantEcommerce.Infrastructure.Outbox;
using MultiTenantEcommerce.Infrastructure.Payments;
using MultiTenantEcommerce.Infrastructure.Persistence.Configurations;
using MultiTenantEcommerce.Infrastructure.Persistence.Context;
using MultiTenantEcommerce.Infrastructure.Persistence.Repositories;
using MultiTenantEcommerce.Infrastructure.Persistence.Seed;
using MultiTenantEcommerce.Infrastructure.Shipping;
using MultiTenantEcommerce.Infrastructure.Storage;
using MultiTenantEcommerce.Infrastructure.Workers;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<TenantContext>();
builder.Services.AddScoped<ITenantContext>(sp => sp.GetRequiredService<TenantContext>());

builder.Services.AddDbContext<AppDbContext>(options =>
   options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("EmployeeOnly", policy =>
    {
        policy.RequireClaim("isEmployee", "true");
        policy.Requirements.Add(new PermissionRequirement());
    });

    options.AddPolicy("CustomerOnly", policy => policy.RequireClaim("isEmployee", "false"));
    options.AddPolicy("SuperAdmin", policy => policy.RequireClaim("superAdmin", "true")); //talvez para mim
    options.AddPolicy("Permission", policy => policy.Requirements.Add(new PermissionRequirement()));
});

builder.Services.AddSingleton<IAuthorizationHandler, PermissionHandler>();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Minha API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Insira o token JWT desta forma: Bearer {seu token}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddRouting();

//Configurações do MediatR
builder.Services.AddMediatR(typeof(ApplicationMarker).Assembly);

//Payment Provider
builder.Services.AddScoped<StripePaymentProvider>();
builder.Services.AddScoped<IPaymentProviderFactory, PaymentProviderFactory>();

//Repositorios
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IStockMovementRepository, StockMovementRepository>();
builder.Services.AddScoped<IStockRepository, StockRepository>();
builder.Services.AddScoped<ITenantRepository, TenantRepository>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<IOrderPaymentRepository, OrderPaymentRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();
builder.Services.AddScoped<IOrderPaymentRepository, OrderPaymentRepository>();
builder.Services.AddScoped<IOutboxRepository, OutboxRepository>();
builder.Services.AddScoped<IProcessedEventsRepository, ProcessedEventsRepository>();
builder.Services.AddScoped<IEmailQueueRepository, EmailQueueRepository>();
builder.Services.AddScoped<IEmailTemplateRepository, EmailTemplateRepository>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));


//Helpers
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<HateoasLinkService>();
builder.Services.AddScoped<PasswordGenerator>();
builder.Services.AddScoped<IShippingProviderFactory, ShippingProviderFactory>();
//builder.WebHost.UseKestrel().UseUrls("http://*:5000");

//Services
builder.Services.AddScoped<IStockService, StockService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IShippingService, ShippingService>();

//Authorization
builder.Services.AddSingleton<IAuthorizationHandler, PermissionHandler>();

//Mappers
builder.Services.AddScoped<CategoryMapper>();
builder.Services.AddScoped<ProductMapper>();
builder.Services.AddScoped<AddressMapper>();
builder.Services.AddScoped<OrderPaymentMapper>();
builder.Services.AddScoped<OrderMapper>();
builder.Services.AddScoped<OrderItemMapper>();
builder.Services.AddScoped<CartMapper>();
builder.Services.AddScoped<TenantMapper>();
builder.Services.AddScoped<CustomerMapper>();
builder.Services.AddScoped<EmployeeMapper>();
builder.Services.AddScoped<StockMapper>();
builder.Services.AddScoped<RolesMapper>();

//Validators
builder.Services.AddScoped<IValidator<CreateProductCommand>, CreateProductCommandValidator>();
builder.Services.AddScoped<IValidator<UpdateProductCommand>, UpdateProductCommandValidator>();
builder.Services.AddScoped<IValidator<CreateCategoryCommand>, CreateCategoryCommandValidator>();
builder.Services.AddScoped<IValidator<UpdateCategoryCommand>, UpdateCategoryCommandValidator>();
builder.Services.AddScoped<IValidator<CreateAddressDTO>, AddressDTOValidator>();
builder.Services.AddScoped<IValidator<CreateTenantCommand>, CreateTenantCommandValidator>();
builder.Services.AddScoped<IValidator<UpdateTenantCommand>, UpdateTenantCommandValidator>();
builder.Services.AddScoped<IValidator<CreateCustomerCommand>, CreateCustomerCommandValidator>();
builder.Services.AddScoped<IValidator<UpdateCustomerCommand>, UpdateCustomerCommandValidator>();
builder.Services.AddScoped<IValidator<CreateEmployeeCommand>, CreateEmployeeCommandValidator>();
builder.Services.AddScoped<IValidator<UpdateEmployeeCommand>, UpdateEmployeeCommandValidator>();
builder.Services.AddHttpClient<IAddressValidator, AddressValidator>();

//HangFire
builder.Services.AddHangfire(config =>
        config.UsePostgreSqlStorage(
            options => options.UseNpgsqlConnection(builder.Configuration.GetConnectionString("DefaultConnection"))));

builder.Services.AddHangfireServer(options => options.SchedulePollingInterval = TimeSpan.FromSeconds(15));

//Events
builder.Services.AddSingleton<EventDispatcher>();
builder.Services.AddSingleton<TemplateRenderer>();

builder.Services.AddScoped<IEventHandler<OrderPaidEvent>, CommitStockOnOrderPaidEventHandler>();
builder.Services.AddScoped<IEventHandler<OrderPaymentFailedEvent>, ReleaseStockOnOrderPaymentFailedEventHandler>();

builder.Services.AddScoped<IEventHandler<LowStockEvent>, SendEmailOnLowStockEventHandler>();
builder.Services.AddScoped<IEventHandler<OutOfStockEvent>, SendEmailOnOutOfStockEventHandler>();
builder.Services.AddScoped<IEventHandler<StockMovementEvent>, StockMovementEventHandler>();

builder.Services.AddScoped<IEventHandler<CustomerRegisteredEvent>, SendEmailOnCustomerRegisteredEventHandler>();
builder.Services.AddScoped<IEventHandler<EmployeeRegisteredEvent>, SendEmailOnEmployeeRegisteredEventHandler>();

builder.Services.AddScoped<IEventHandler<OrderPaidEvent>, SendEmailOnOrderPaidEventHandler>();
builder.Services.AddScoped<IEventHandler<OrderPaymentFailedEvent>, SendEmailOnOrderPaymentFailedEventHandler>();

builder.Services.AddScoped<IEventHandler<ShipmentDeliveredEvent>, SendEmailOnShipmentDeliveredEventHandler>();
builder.Services.AddScoped<IEventHandler<ShipmentShippedEvent>, SendEmailOnShipmentShippedEventHandler>();

//Email
builder.Services.AddSingleton<IEmailSender, EmailSender>();

//RabbitMq
builder.Services.AddSingleton<IEventBus, RabbitMqEventBus>();
builder.Services.AddSingleton<RabbitMqConnectionManager>();

builder.Services.AddHostedService<CriticalConsumer>();
builder.Services.AddHostedService<NonCriticalConsumer>();

//File Storage
builder.Services.AddScoped<IFileStorageService, FileStorageService>();

//Processors
builder.Services.AddScoped<OutboxProcessor>();
builder.Services.AddScoped<EmailProcessor>();

////workers
//builder.Services.AddHostedService(sp =>
//{
//    var config = sp.GetRequiredService<IConfiguration>();
//    return new GenericWorker<OutboxProcessor>(
//        TimeSpan.FromSeconds(config.GetValue<int>("Workers:PollingIntervalSecondsOutboxCritical")),
//        sp,
//        EventPriority.Critical);
//});
//builder.Services.AddHostedService(sp =>
//{
//    var config = sp.GetRequiredService<IConfiguration>();
//    return new GenericWorker<EmailProcessor>(
//        TimeSpan.FromSeconds(config.GetValue<int>("Workers:PollingIntervalSecondsOutboxCritical")),
//        sp,
//        EventPriority.Critical);
//});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHangfireDashboard();
}

await SeedPermissions.InitializeAsync(app.Services);
await EmailTemplateSeeder.SeedAsync(app.Services, "C:\\MultiTenantEcommerce\\MultiTenantEcommerce\\MultiTenantEcommerce.Infrastructure\\EmailTemplates\\");


//Jobs
RecurringJob.AddOrUpdate<OutboxProcessor>
    ("NonCritical-OutboxProcessor", service =>
    service.ExecuteAsync(EventPriority.NonCritical), "0 0 * * * *");

RecurringJob.AddOrUpdate<EmailProcessor>
    ("NonCritical-EmailProcessor", service =>
    service.ExecuteAsync(EventPriority.NonCritical), "0 0 * * * *");

app.UseHttpsRedirection();

app.UseMiddleware<TenantMiddleware>();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

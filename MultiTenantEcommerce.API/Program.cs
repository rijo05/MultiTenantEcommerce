using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MultiTenantEcommerce.API.Middleware;
using MultiTenantEcommerce.Application.DTOs.Category;
using MultiTenantEcommerce.Application.DTOs.Employees;
using MultiTenantEcommerce.Application.DTOs.Product;
using MultiTenantEcommerce.Application.Interfaces;
using MultiTenantEcommerce.Application.Validators.CategoryValidator;
using MultiTenantEcommerce.Application.Validators.ProductValidator;
using MultiTenantEcommerce.Application.Validators.OrderItemValidator;
using MultiTenantEcommerce.Application.Validators.Common;
using MultiTenantEcommerce.Application.Mappers;
using MultiTenantEcommerce.Application.Services;
using MultiTenantEcommerce.Domain.Events.Products;
using MultiTenantEcommerce.Domain.Interfaces;
using MultiTenantEcommerce.Infrastructure.Configurations;
using MultiTenantEcommerce.Infrastructure.Context;
using MultiTenantEcommerce.Infrastructure.Repositories;
using MultiTenantEcommerce.Application.DTOs.Order;
using MultiTenantEcommerce.Application.Validators.OrderValidator;
using MultiTenantEcommerce.Application.Validators.EmployeeValidator;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddRouting();

//Configurações do MediatR
builder.Services.AddMediatR(typeof(ProductOutOfStockEvent).Assembly);

//Repositorios
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IOrderItemRepository, OrderItemRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IStockMovementRepository, StockMovementRepository>();
builder.Services.AddScoped<IStockRepository, StockRepository>();
builder.Services.AddScoped<ITenantRepository, TenantRepository>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));


//Helpers
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<TenantContext>();
builder.Services.AddScoped<HateoasLinkService>();

//Employee Service
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<EmployeeMapper>();
builder.Services.AddScoped<IValidator<CreateEmployeeDTO>, CreateEmployeeDTOValidator>();
builder.Services.AddScoped<IValidator<UpdateEmployeeDTO>, UpdateEmployeeDTOValidator>();

//Product Service
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ProductMapper>();
builder.Services.AddScoped<IValidator<CreateProductDTO>, CreateProductDTOValidator>();
builder.Services.AddScoped<IValidator<UpdateProductDTO>, UpdateProductDTOValidator>();

//Category Service
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<CategoryMapper>();
builder.Services.AddScoped<IValidator<CreateCategoryDTO>, CreateCategoryDTOValidator>();
builder.Services.AddScoped<IValidator<UpdateCategoryDTO>, UpdateCategoryDTOValidator>();

//Order Service
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<OrderMapper>();
builder.Services.AddScoped<AddressMapper>();
builder.Services.AddScoped<OrderItemMapper>();
builder.Services.AddScoped<IValidator<CreateOrderDTO>, CreateOrderDTOValidator>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<TenantMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();

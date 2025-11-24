using MultiTenantEcommerce.Application.Catalog.Products.Commands.Create;
using MultiTenantEcommerce.Application.Catalog.Products.DTOs.Products;
using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;

namespace MultiTenantEcommerce.Application.Catalog.Products.Commands.CreateBulk;
public record CreateProductBulkCommand(
    IEnumerable<CreateProductCommand> Products) : ICommand<List<ProductResponseAdminDTO>>;

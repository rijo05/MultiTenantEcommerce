using MediatR;
using MultiTenantEcommerce.Application.Catalog.Products.Commands.Create;
using MultiTenantEcommerce.Application.Catalog.Products.DTOs;
using MultiTenantEcommerce.Application.Common.Interfaces;

namespace MultiTenantEcommerce.Application.Catalog.Products.Commands.CreateBulk;
public record CreateProductBulkCommand(
    IEnumerable<CreateProductCommand> Products) : ICommand<List<ProductResponseDTO>>;

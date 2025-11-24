using MediatR;
using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Application.Catalog.Products.Commands.DeleteImage;
public record DeleteProductImageCommand(
    Guid ProductId,
    string Key) : ICommand<Unit>;

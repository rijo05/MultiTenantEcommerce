using MultiTenantEcommerce.Application.Catalog.Products.DTOs;
using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Domain.Catalog.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MultiTenantEcommerce.Application.Catalog.Products.Commands.SetMainImage;
public record SetMainProductImageCommand(
    Guid ProductId,
    string Key) : ICommand<Product>;

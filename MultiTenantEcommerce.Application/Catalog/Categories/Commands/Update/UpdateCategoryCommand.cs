using MultiTenantEcommerce.Application.Catalog.Categories.DTOs;
using MultiTenantEcommerce.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Application.Catalog.Categories.Commands.Update;
public record UpdateCategoryCommand(
    Guid CategoryId,
    string? Name,
    string? Description,
    bool? IsActive) : ICommand<CategoryResponseDTO>;
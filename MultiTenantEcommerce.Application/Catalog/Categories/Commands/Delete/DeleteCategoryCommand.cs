using MediatR;
using MultiTenantEcommerce.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Application.Catalog.Categories.Commands.Delete;
public record DeleteCategoryCommand(
    Guid id) : ICommand<Unit>;

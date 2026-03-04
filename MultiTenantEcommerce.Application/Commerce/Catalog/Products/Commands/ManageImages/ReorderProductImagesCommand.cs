using MediatR;
using MultiTenantEcommerce.Shared.Application.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Application.Commerce.Catalog.Products.Commands.ManageImages;
public record ReorderProductImagesCommand(
    Guid ProductId, 
    List<Guid> OrderedImageIds) : ICommand<Unit>;

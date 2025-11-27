using MultiTenantEcommerce.Application.Catalog.Products.DTOs.Images;
using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Application.Catalog.Products.Queries.GetImage;
public record GetProductImageByKeyAndProductIdQuery(
    Guid ProductId,
    string Key,
    bool IsAdmin) : IQuery<IProductImageDTO>;

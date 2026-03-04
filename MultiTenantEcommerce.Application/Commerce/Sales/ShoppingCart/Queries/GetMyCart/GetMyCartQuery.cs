using MultiTenantEcommerce.Application.Commerce.Sales.ShoppingCart.Common.DTOs;
using MultiTenantEcommerce.Shared.Application.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Application.Commerce.Sales.ShoppingCart.Queries.GetMyCart;
public record GetMyCartQuery(Guid? CustomerId, Guid? AnonymousId) : IQuery<CartDetailDTO>;

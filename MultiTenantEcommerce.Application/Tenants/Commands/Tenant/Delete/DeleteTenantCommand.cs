using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Application.Tenants.Commands.Tenant.Delete;
public record DeleteTenantCommand(
    Guid Id) : IRequest<Unit>;
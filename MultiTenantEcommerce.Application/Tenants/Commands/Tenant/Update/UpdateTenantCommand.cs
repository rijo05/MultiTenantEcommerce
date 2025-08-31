using MediatR;
using MultiTenantEcommerce.Application.Tenants.DTOs.Tenant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Application.Tenants.Commands.Tenant.Update;
public record UpdateTenantCommand(
    Guid Id, 
    string CompanyName) : IRequest<TenantResponseDTO>;

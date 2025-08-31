using MediatR;
using MultiTenantEcommerce.Application.Tenants.DTOs.Tenant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Application.Tenants.Commands.Tenant.Create;
public record CreateTenantCommand(
    string CompanyName) : IRequest<TenantResponseDTO>;

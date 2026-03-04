using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Application.Platform.Tenancy.Tenants.Queries.GetMyWorkspaces;
public record UserWorkspaceDTO(Guid TenantId, string TenantName, string Subdomain, bool IsOwner);

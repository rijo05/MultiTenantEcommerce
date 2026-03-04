using MultiTenantEcommerce.Shared.Application.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Application.Platform.Tenancy.Tenants.Queries.GetMyWorkspaces;
public record GetMyWorkspacesQuery(
    Guid UserId, 
    bool IsPlatformAdmin) : IQuery<List<UserWorkspaceDTO>>;
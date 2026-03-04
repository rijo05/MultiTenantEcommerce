using MultiTenantEcommerce.Shared.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Domain.Platform.Tenancy.Interfaces.Services;
public interface ITenantMemberPolicy
{
    public Task EnsureCanAddMemberAsync(Guid? userId, Email email, int maxMembers);
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Shared.Domain.Events;
public interface IEvent
{
    Guid EventId { get; }
    Guid TenantId { get; }
    DateTime OccurredOn { get; }
}

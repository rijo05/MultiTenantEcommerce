using MediatR;

namespace MultiTenantEcommerce.Domain.Common;

//Interface para os eventos, herda o INotification do MediatR

public interface IDomainEvent : INotification
{
    DateTime OccurredOn { get; }
}

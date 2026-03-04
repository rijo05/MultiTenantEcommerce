using MediatR;
using MultiTenantEcommerce.Shared.Application.CQRS;

namespace MultiTenantEcommerce.Application.Commerce.Customers.Commands.ManageAddresses;

public record RemoveAddressCommand(Guid CustomerId, Guid AddressId) : ICommand<Unit>;
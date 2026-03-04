using MediatR;
using MultiTenantEcommerce.Application.Common.DTOs.Address;
using MultiTenantEcommerce.Shared.Application.CQRS;

namespace MultiTenantEcommerce.Application.Commerce.Customers.Commands.ManageAddresses;

public record AddAddressCommand(Guid CustomerId, CreateAddressDTO AddressDTO) : ICommand<Unit>;
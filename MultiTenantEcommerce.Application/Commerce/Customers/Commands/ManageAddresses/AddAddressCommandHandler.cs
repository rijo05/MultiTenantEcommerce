using MediatR;
using MultiTenantEcommerce.Domain.Commerce.Customers.Entities;
using MultiTenantEcommerce.Domain.Commerce.Customers.Interfaces;
using MultiTenantEcommerce.Shared.Application.CQRS;
using MultiTenantEcommerce.Shared.Domain.ValueObjects;
using MultiTenantEcommerce.Shared.Infrastructure.Persistence;

namespace MultiTenantEcommerce.Application.Commerce.Customers.Commands.ManageAddresses;
public class AddAddressCommandHandler : ICommandHandler<AddAddressCommand, Unit>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddAddressCommandHandler(ICustomerRepository customerRepository, 
        IUnitOfWork unitOfWork)
    {
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(AddAddressCommand request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByIdAsync(request.CustomerId)
            ?? throw new Exception("User doesnt exist");

        customer.AddAddress(request.AddressDTO.Street,
            request.AddressDTO.City,
            request.AddressDTO.PostalCode,
            request.AddressDTO.Country,
            request.AddressDTO.HouseNumber);

        await _unitOfWork.CommitAsync();

        return Unit.Value;
    }
}

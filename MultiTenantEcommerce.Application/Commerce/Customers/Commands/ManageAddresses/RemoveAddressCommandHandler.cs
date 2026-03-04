using MediatR;
using MultiTenantEcommerce.Domain.Commerce.Customers.Interfaces;
using MultiTenantEcommerce.Shared.Application.CQRS;
using MultiTenantEcommerce.Shared.Infrastructure.Persistence;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Application.Commerce.Customers.Commands.ManageAddresses;
public class RemoveAddressCommandHandler : ICommandHandler<RemoveAddressCommand, Unit>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RemoveAddressCommandHandler(ICustomerRepository customerRepository, 
        IUnitOfWork unitOfWork)
    {
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(RemoveAddressCommand request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByIdAsync(request.CustomerId) 
            ?? throw new Exception("User doesnt exist");

        customer.RemoveAddress(request.AddressId);

        await _unitOfWork.CommitAsync();

        return Unit.Value;
    }
}

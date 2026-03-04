using MediatR;
using MultiTenantEcommerce.Domain.Commerce.Customers.Interfaces;
using MultiTenantEcommerce.Shared.Application.CQRS;
using MultiTenantEcommerce.Shared.Domain.ValueObjects;
using MultiTenantEcommerce.Shared.Infrastructure.Persistence;

namespace MultiTenantEcommerce.Application.Commerce.Customers.Commands.Update;

public class UpdateCustomerCommandHandler : ICommandHandler<UpdateCustomerCommand, Unit>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCustomerCommandHandler(
        ICustomerRepository customerRepository,
        IUnitOfWork unitOfWork)
    {
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByIdAsync(request.Id)
                       ?? throw new Exception("Customer not found.");

        if (!string.IsNullOrWhiteSpace(request.Email))
        {
            var existingEmail = await _customerRepository.GetByEmailAsync(new Email(request.Email));
            if (existingEmail != null && existingEmail.Id != customer.Id)
                throw new Exception("Email already in use.");
        }


        customer.UpdateCustomer(request.Name,
            request.Email,
            request.Password);

        //TODO() ver como atualizar address e phone number ##########

        await _unitOfWork.CommitAsync();
        return Unit.Value;
    }
}
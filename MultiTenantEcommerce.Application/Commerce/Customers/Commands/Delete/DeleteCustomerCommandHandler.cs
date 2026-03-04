using MediatR;
using MultiTenantEcommerce.Domain.Commerce.Customers.Interfaces;
using MultiTenantEcommerce.Shared.Application.CQRS;
using MultiTenantEcommerce.Shared.Infrastructure.Persistence;

namespace MultiTenantEcommerce.Application.Commerce.Customers.Commands.Delete;

public class DeleteCustomerCommandHandler : ICommandHandler<DeleteCustomerCommand, Unit>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCustomerCommandHandler(ICustomerRepository customerRepository, IUnitOfWork unitOfWork)
    {
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByIdAsync(request.Id)
                       ?? throw new Exception("Customer not found.");

        await _customerRepository.DeleteAsync(customer);
        await _unitOfWork.CommitAsync();

        return Unit.Value;
    }
}
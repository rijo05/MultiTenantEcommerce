using MediatR;
using MultiTenantEcommerce.Domain.Commerce.Customers.Interfaces;
using MultiTenantEcommerce.Domain.Commerce.Customers.Entities;
using MultiTenantEcommerce.Shared.Application.Auth;
using MultiTenantEcommerce.Shared.Application.CQRS;
using MultiTenantEcommerce.Shared.Domain.ValueObjects;
using MultiTenantEcommerce.Shared.Infrastructure.Persistence;

namespace MultiTenantEcommerce.Application.Commerce.Customers.Commands.Auth.CreateCustomer;

public class CreateCustomerCommandHandler : ICommandHandler<CreateCustomerCommand, Unit>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly ITenantContext _tenantContext;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _hasher;

    public CreateCustomerCommandHandler(
        ICustomerRepository customerRepository,
        IUnitOfWork unitOfWork,
        ITenantContext tenantContext, 
        IPasswordHasher hasher)
    {
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
        _tenantContext = tenantContext;
        _hasher = hasher;
    }

    public async Task<Unit> Handle(CreateCustomerCommand request,
        CancellationToken cancellationToken)
    {
        if (await _customerRepository.CheckEmailInUse(new Email(request.Email)))
            throw new Exception("Customer with this email already exists.");

        var customer = new Customer(
            _tenantContext.TenantId,
            request.FirstName,
            request.LastName,
            new Email(request.Email),
            _hasher.Hash(request.Password));

        await _customerRepository.AddAsync(customer);
        await _unitOfWork.CommitAsync();

        return Unit.Value;
    }
}
using MediatR;
using MultiTenantEcommerce.Domain.Commerce.Customers.Entities;
using MultiTenantEcommerce.Domain.Commerce.Customers.Interfaces;
using MultiTenantEcommerce.Domain.Platform.Identity.Entities;
using MultiTenantEcommerce.Domain.Platform.Identity.Interfaces;
using MultiTenantEcommerce.Shared.Application.Auth;
using MultiTenantEcommerce.Shared.Application.CQRS;
using MultiTenantEcommerce.Shared.Domain.ValueObjects;
using MultiTenantEcommerce.Shared.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Application.Platform.Identity.Commands.Auth.CreateUser;
public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, Unit>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _hasher;
    private readonly IUnitOfWork _unitOfWork;

    public CreateUserCommandHandler(IUserRepository userRepository, 
        IPasswordHasher hasher, 
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _hasher = hasher;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        if (await _userRepository.ExistsByEmailAsync(new Email(request.Email)))
            throw new Exception("User with this email already exists.");

        var customer = new User(
            request.FirstName,
            request.LastName,
            new Email(request.Email),
            _hasher.Hash(request.Password),
            false);

        await _userRepository.AddAsync(customer);
        await _unitOfWork.CommitAsync();

        return Unit.Value;
    }
}

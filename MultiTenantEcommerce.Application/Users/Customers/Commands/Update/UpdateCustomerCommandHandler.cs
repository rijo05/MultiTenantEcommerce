using MediatR;
using MultiTenantEcommerce.Application.Common.Interfaces;
using MultiTenantEcommerce.Application.Users.Customers.DTOs;
using MultiTenantEcommerce.Application.Users.Customers.Mappers;
using MultiTenantEcommerce.Domain.Common.Interfaces;
using MultiTenantEcommerce.Domain.Users.Interfaces;
using MultiTenantEcommerce.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Application.Users.Customers.Commands.Update;
public class UpdateCustomerCommandHandler : ICommandHandler<UpdateCustomerCommand, CustomerResponseDTO>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly CustomerMapper _mapper;

    public UpdateCustomerCommandHandler(
        ICustomerRepository customerRepository,
        IUnitOfWork unitOfWork,
        CustomerMapper mapper)
    {
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CustomerResponseDTO> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByIdAsync(request.Id)
            ?? throw new Exception("Customer not found.");

        if (!string.IsNullOrWhiteSpace(request.Email))
        {
            var existingEmail = await _customerRepository.GetByEmailAsync(new Email(request.Email));
            if (existingEmail != null && existingEmail.Id != customer.Id)
                throw new Exception("Email already in use.");
        }
        if (!string.IsNullOrWhiteSpace(request.PhoneNumber))
        {
            var existingPhone = await _customerRepository.GetByPhoneNumberAsync(new PhoneNumber(request.CountryCode ?? "000", request.PhoneNumber));
            if (existingPhone != null && existingPhone.Id != customer.Id)
                throw new Exception("Phone number already in use.");
        }


        customer.UpdateCustomer(request.Name,
            request.Email,
            request.Password);

        //TODO() ver como atualizar address e phone number ##########

        await _unitOfWork.CommitAsync();
        return _mapper.ToCustomerResponseDTO(customer);
    }
}

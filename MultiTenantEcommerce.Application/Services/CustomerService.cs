using FluentValidation;
using MultiTenantEcommerce.Application.DTOs.Customer;
using MultiTenantEcommerce.Application.Interfaces;
using MultiTenantEcommerce.Application.Mappers;
using MultiTenantEcommerce.Application.Validators.Common;
using MultiTenantEcommerce.Domain.Common.Interfaces;
using MultiTenantEcommerce.Domain.Users.Entities;
using MultiTenantEcommerce.Domain.Users.Interfaces;
using MultiTenantEcommerce.Domain.ValueObjects;
using MultiTenantEcommerce.Infrastructure.Persistence.Context;


namespace MultiTenantEcommerce.Application.Services;
public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly CustomerMapper _customerMapper;
    private readonly IValidator<CreateCustomerDTO> _validatorCreate;
    private readonly IValidator<UpdateCustomerDTO> _validatorUpdate;
    private readonly IUnitOfWork _unitOfWork;
    private readonly TenantContext _tenantContext;

    public CustomerService(ICustomerRepository customerRepository, 
        CustomerMapper customerMapper, 
        IValidator<CreateCustomerDTO> validatorCreate,
        IValidator<UpdateCustomerDTO> validatorUpdate,
        IUnitOfWork unitOfWork,
        TenantContext tenantContext) 
    { 
        _customerRepository = customerRepository;
        _customerMapper = customerMapper;
        _validatorCreate = validatorCreate;
        _validatorUpdate = validatorUpdate;
        _unitOfWork = unitOfWork;
        _tenantContext = tenantContext;
    }

    public async Task<CustomerResponseDTO?> GetCustomerByIdAsync(Guid id)
    {
        var customer = await EnsureCustomerExists(id);

        return _customerMapper.ToCustomerResponseDTO(customer);
    }

    public async Task<CustomerResponseDTO?> GetCustomerByEmailAsync(string email)
    {
        var customer = await _customerRepository.GetByEmailAsync(new Email(email));

        if (customer is null)
            throw new Exception($"Customer with {email} email not found");

        return _customerMapper.ToCustomerResponseDTO(customer);
    }

    public async Task<CustomerResponseDTO?> GetCustomerByPhoneNumberAsync(string number)
    {
        var customer = await _customerRepository.GetByPhoneNumberAsync(new PhoneNumber(countryCode: "000", Number: number));

        if (customer is null)
            throw new Exception($"Customer with {number} phone number not found");

        return _customerMapper.ToCustomerResponseDTO(customer);
    }

    public async Task<CustomerResponseDTO> AddCustomerAsync(CreateCustomerDTO customerDTO)
    {
        await ValidationRules.ValidateAsync(customerDTO, _validatorCreate);


        //Verificar se o email já está a ser usado por outro Customer
        var existingCustomerEmail = await _customerRepository.GetByEmailAsync(new Email(customerDTO.Email));
        if (existingCustomerEmail is not null)
            throw new Exception("Customer with this email already exists.");

        //Verificar se o numero de telemovel já está a ser usado por outro Customer
        var existingCustomerPhone = await _customerRepository.GetByPhoneNumberAsync(new PhoneNumber(customerDTO.CountryCode, customerDTO.PhoneNumber));
        if (existingCustomerPhone is not null)
            throw new Exception("Customer with this email already exists.");


        var customer = new Customer(_tenantContext.TenantId, 
            customerDTO.Name, 
            new Email(customerDTO.Email), 
            new Password(customerDTO.Password),
            new Address(customerDTO.Address.Street,
            customerDTO.Address.City,
            customerDTO.Address.PostalCode,
            customerDTO.Address.Country,
            customerDTO.Address.HouseNumber),
            new PhoneNumber(customerDTO.CountryCode, customerDTO.PhoneNumber));

        await _customerRepository.AddAsync(customer);
        await _unitOfWork.CommitAsync();
        return _customerMapper.ToCustomerResponseDTO(customer);
    }

    public async Task DeleteCustomerAsync(Guid id)
    {
        var customer = await EnsureCustomerExists(id);

        await _customerRepository.DeleteAsync(customer);
        await _unitOfWork.CommitAsync();
    }

    public async Task<CustomerResponseDTO> UpdateCustomerAsync(Guid id, UpdateCustomerDTO updatedEmployeeDTO)
    {
        await ValidationRules.ValidateAsync(updatedEmployeeDTO, _validatorUpdate);


        //TODO() ################################
        throw new NotImplementedException();
    }

    private async Task<Customer?> EnsureCustomerExists(Guid id)
    {
        return await _customerRepository.GetByIdAsync(id) ?? throw new Exception("Customer doesn't exist.");
    }
}

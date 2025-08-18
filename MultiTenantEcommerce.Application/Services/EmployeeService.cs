using FluentValidation;
using MultiTenantEcommerce.Application.DTOs.Employees;
using MultiTenantEcommerce.Application.Interfaces;
using MultiTenantEcommerce.Application.Mappers;
using MultiTenantEcommerce.Domain.Entities;
using MultiTenantEcommerce.Domain.Interfaces;
using MultiTenantEcommerce.Domain.ValueObjects;
using MultiTenantEcommerce.Infrastructure.Context;

namespace MultiTenantEcommerce.Application.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmployeeRepository _EmployeeRepository;
    private readonly IValidator<CreateEmployeeDTO> _validatorCreate;
    private readonly IValidator<UpdateEmployeeDTO> _validatorUpdate;
    private readonly EmployeeMapper _Employeemapper;
    private readonly TenantContext _tenantContext;

    public EmployeeService(IEmployeeRepository EmployeeRepository, 
        IUnitOfWork unitOfWork, 
        IValidator<CreateEmployeeDTO> validatorCreate, 
        IValidator<UpdateEmployeeDTO> validatorUpdate, 
        EmployeeMapper EmployeeMapper, 
        TenantContext tenantContext)
    {
        _unitOfWork = unitOfWork;
        _EmployeeRepository = EmployeeRepository;
        _validatorCreate = validatorCreate;
        _validatorUpdate = validatorUpdate;
        _Employeemapper = EmployeeMapper;
        _tenantContext = tenantContext;
    }

    public async Task<List<EmployeeResponseDTO>> GetFilteredEmployeesAsync(EmployeeFilterDTO EmployeeFilter)
    {
        var Employees = await _EmployeeRepository.GetFilteredAsync(EmployeeFilter.Name,
            EmployeeFilter.Role,
            EmployeeFilter.Email,
            EmployeeFilter.IsActive,
            EmployeeFilter.Page,
            EmployeeFilter.PageSize,
            EmployeeFilter.Sort);

        return _Employeemapper.ToEmployeeResponseDTOList(Employees);
    }

    public async Task<EmployeeResponseDTO?> GetEmployeeByEmailAsync(string email)
    {
        var Employee = await _EmployeeRepository.GetByEmailAsync(new Email(email));

        if (Employee is null)
            throw new Exception($"Employee with {email} email not found");

        return _Employeemapper.ToEmployeeResponseDTO(Employee);
    }

    public async Task<EmployeeResponseDTO?> GetEmployeeByIdAsync(Guid id)
    {
        var Employee =  await EnsureEmployeeExists(id);

        return _Employeemapper.ToEmployeeResponseDTO(Employee);
    }

    public async Task<EmployeeResponseDTO> AddEmployeeAsync(CreateEmployeeDTO EmployeeDTO)
    {
        //Validar os dados
        var validationResult = await _validatorCreate.ValidateAsync(EmployeeDTO);
        if (!validationResult.IsValid) 
            throw new ValidationException(validationResult.Errors);


        //Verificar se o email já está a ser usado por outro Employee
        var existingEmployee = await _EmployeeRepository.GetByEmailAsync(new Email(EmployeeDTO.Email));
        if (existingEmployee is not null) 
            throw new Exception("Employee with this email already exists.");


        var Employee = new Employee(_tenantContext.TenantId, EmployeeDTO.Name, new Email(EmployeeDTO.Email), new Role(EmployeeDTO.Role), new Password(EmployeeDTO.Password));
        await _EmployeeRepository.AddAsync(Employee);
        await _unitOfWork.CommitAsync();
        return _Employeemapper.ToEmployeeResponseDTO(Employee);
    }

    public async Task<EmployeeResponseDTO> UpdateEmployeeAsync(Guid id, UpdateEmployeeDTO updateEmployeeDTO)
    {
        //Ver se o Employee realmente existe
        var Employee = await EnsureEmployeeExists(id);


        //Ver se já existe um Employee com o email novo
        if (!string.IsNullOrWhiteSpace(updateEmployeeDTO.Email))
        {
            var existingEmail = await _EmployeeRepository.GetByEmailAsync(new Email(updateEmployeeDTO.Email));
            if (existingEmail is not null && existingEmail.Id != Employee.Id)
                throw new Exception("Email already in use.");
        }

        //Validar DTO
        var validationResult = await _validatorUpdate.ValidateAsync(updateEmployeeDTO);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);



        if (!string.IsNullOrWhiteSpace(updateEmployeeDTO.Name))
            Employee.UpdateName(updateEmployeeDTO.Name);

        if (updateEmployeeDTO.Email is not null)
            Employee.UpdateEmail(updateEmployeeDTO.Email);

        if (updateEmployeeDTO.Password is not null)
            Employee.UpdatePassword(updateEmployeeDTO.Password);

        if (updateEmployeeDTO.IsActive.HasValue)
            Employee.SetActive(updateEmployeeDTO.IsActive.Value);

        if (updateEmployeeDTO.Role != null)
            Employee.UpdateRole(updateEmployeeDTO.Role);


        await _unitOfWork.CommitAsync();
        return _Employeemapper.ToEmployeeResponseDTO(Employee);
    }

    public async Task DeleteEmployeeAsync(Guid id)
    {
        //Ver se o Employee existe 
        var Employee = await EnsureEmployeeExists(id);

        await _EmployeeRepository.DeleteAsync(Employee);
        await _unitOfWork.CommitAsync();
    }

    private async Task<Employee?> EnsureEmployeeExists(Guid id)
    {
        return await _EmployeeRepository.GetByIdAsync(id) ?? throw new Exception("Employee doesn't exist.");
    }
}

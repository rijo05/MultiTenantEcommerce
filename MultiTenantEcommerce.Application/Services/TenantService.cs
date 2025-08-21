using FluentValidation;
using MultiTenantEcommerce.Application.DTOs.Employees;
using MultiTenantEcommerce.Application.DTOs.Tenant;
using MultiTenantEcommerce.Application.Interfaces;
using MultiTenantEcommerce.Application.Mappers;
using MultiTenantEcommerce.Domain.Entities;
using MultiTenantEcommerce.Domain.Interfaces;
using MultiTenantEcommerce.Domain.ValueObjects;
using MultiTenantEcommerce.Infrastructure.Context;
using MultiTenantEcommerce.Infrastructure.Repositories;

namespace MultiTenantEcommerce.Application.Services;
public class TenantService : ITenantService
{
    private readonly ITenantRepository _tenantRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly TenantMapper _tenantMapper;
    private readonly IValidator<CreateTenantDTO> _validatorCreate;
    private readonly IValidator<UpdateTenantDTO> _validatorUpdate;
    public TenantService(ITenantRepository tenantRepository, IUnitOfWork unitOfWork, IValidator<CreateTenantDTO> validatorCreate, TenantMapper tenantMapper, IValidator<UpdateTenantDTO> validatorUpdate)
    {
        _tenantRepository = tenantRepository;
        _unitOfWork = unitOfWork;
        _tenantMapper = tenantMapper;
        _validatorCreate = validatorCreate;
        _validatorUpdate = validatorUpdate;
    }

    public async Task<TenantResponseDTO?> GetTenantByIdAsync(Guid id)
    {
        var tenant = await _tenantRepository.GetByIdAsync(id);

        if (tenant == null)
            throw new Exception("Tenant doesnt exist");

        return _tenantMapper.ToTenantResponseDTO(tenant);
    }

    public async Task<List<TenantResponseDTO>> GetFilteredTenantsAsync(TenantFilterDTO tenantFilterDTO)
    {
        var tenants = await _tenantRepository.GetFilteredAsync(
            tenantFilterDTO.Page,
            tenantFilterDTO.PageSize,
            tenantFilterDTO.Sort);

        return _tenantMapper.ToTenantResponseDTOList(tenants);
    }


    public async Task<TenantResponseDTO> AddTenantAsync(CreateTenantDTO tenantDTO)
    {
        //Validar os dados
        var validationResult = await _validatorCreate.ValidateAsync(tenantDTO);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);


        //Verificar se o email já está a ser usado por outro Employee
        var existingTenant = await _tenantRepository.GetByCompanyName(tenantDTO.CompanyName);
        if (existingTenant is not null)
            throw new Exception("Company with this name already exists.");

        var tenant = new Tenant(tenantDTO.CompanyName);

        await _tenantRepository.AddAsync(tenant);
        await _unitOfWork.CommitAsync();
        return _tenantMapper.ToTenantResponseDTO(tenant);
    }

    public async Task DeleteTenantAsync(Guid id)
    {
        var tenant = await EnsureTenantExists(id);

        await _tenantRepository.DeleteAsync(tenant);
        await _unitOfWork.CommitAsync();
    }

    public async Task<TenantResponseDTO> UpdateTenantAsync(Guid id, UpdateTenantDTO updatedTenant)
    {
        var tenant = await EnsureTenantExists(id);

        var existingTenant = await _tenantRepository.GetByCompanyName(updatedTenant.CompanyName);
        if (existingTenant is not null)
            throw new Exception("Company with this name already exists.");

        var validationResult = await _validatorUpdate.ValidateAsync(updatedTenant);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        if (!string.IsNullOrEmpty(updatedTenant.CompanyName))
            tenant.UpdateCompanyName(updatedTenant.CompanyName);

        await _unitOfWork.CommitAsync();
        return _tenantMapper.ToTenantResponseDTO(tenant);
    }

    private async Task<Tenant?> EnsureTenantExists(Guid id)
    {
        return await _tenantRepository.GetByIdAsync(id) ?? throw new Exception("Tenant doesn't exist.");
    }
}

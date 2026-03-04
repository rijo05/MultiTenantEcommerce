using MediatR;
using MultiTenantEcommerce.Application.Platform.Tenancy.Tenants.Common.DTOs;
using MultiTenantEcommerce.Application.Platform.Tenancy.Tenants.Common.Mappers;
using MultiTenantEcommerce.Domain.Platform.Tenancy.Interfaces.Repositories;

namespace MultiTenantEcommerce.Application.Platform.Tenancy.Tenants.Commands.UpdateSettings;

public class UpdateTenantCommandHandler : IRequestHandler<UpdateTenantCommand, TenantResponseDTO>
{
    private readonly TenantMapper _mapper;
    private readonly ITenantRepository _tenantRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateTenantCommandHandler(ITenantRepository tenantRepository, IUnitOfWork unitOfWork, TenantMapper mapper)
    {
        _tenantRepository = tenantRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<TenantResponseDTO> Handle(UpdateTenantCommand request, CancellationToken cancellationToken)
    {
        var tenant = await _tenantRepository.GetByIdAsync(request.Id)
                     ?? throw new Exception("Tenant doesn't exist.");

        var existingTenant = await _tenantRepository.GetByCompanyNameAllIncluded(request.CompanyName);
        if (existingTenant is not null && existingTenant.Id != request.Id)
            throw new Exception("Company with this name already exists.");


        tenant.UpdateTenant(request.CompanyName);

        await _unitOfWork.CommitAsync();

        return _mapper.ToTenantResponseDTO(tenant);
    }
}
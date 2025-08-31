using MediatR;
using MultiTenantEcommerce.Application.Tenants.DTOs.Tenant;
using MultiTenantEcommerce.Application.Tenants.Mappers;
using MultiTenantEcommerce.Domain.Common.Interfaces;
using MultiTenantEcommerce.Domain.Tenants.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Application.Tenants.Commands.Tenant.Update;
public class UpdateTenantCommandHandler : IRequestHandler<UpdateTenantCommand, TenantResponseDTO>
{
    private readonly ITenantRepository _tenantRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly TenantMapper _mapper;

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

        var existingTenant = await _tenantRepository.GetByCompanyName(request.CompanyName);
        if (existingTenant is not null && existingTenant.Id != request.Id)
            throw new Exception("Company with this name already exists.");


        tenant.UpdateTenant(request.CompanyName);

        await _unitOfWork.CommitAsync();

        return _mapper.ToTenantResponseDTO(tenant);
    }
}

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

namespace MultiTenantEcommerce.Application.Tenants.Commands.Tenant.Create;
public class CreateTenantCommandHandler : IRequestHandler<CreateTenantCommand, TenantResponseDTO>
{
    private readonly ITenantRepository _tenantRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly TenantMapper _mapper;

    public CreateTenantCommandHandler(ITenantRepository tenantRepository, 
        IUnitOfWork unitOfWork, 
        TenantMapper mapper)
    {
        _tenantRepository = tenantRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<TenantResponseDTO> Handle(CreateTenantCommand request, CancellationToken cancellationToken)
    {
        var existingTenant = await _tenantRepository.GetByCompanyName(request.CompanyName) 
            ?? throw new Exception("Company with this name already exists."); ;

        var tenant = new Domain.Tenancy.Entities.Tenant(request.CompanyName);

        await _tenantRepository.AddAsync(tenant);
        await _unitOfWork.CommitAsync();

        return _mapper.ToTenantResponseDTO(tenant);
    }
}

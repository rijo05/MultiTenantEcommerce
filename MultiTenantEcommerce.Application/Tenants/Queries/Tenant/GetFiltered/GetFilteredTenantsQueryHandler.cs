using MediatR;
using MultiTenantEcommerce.Application.Tenants.DTOs.Tenant;
using MultiTenantEcommerce.Application.Tenants.Mappers;
using MultiTenantEcommerce.Domain.Tenants.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Application.Tenants.Queries.Tenant.GetFiltered;
public class GetFilteredTenantsQueryHandler : IRequestHandler<GetFilteredTenantsQuery, List<TenantResponseDTO>>
{
    private readonly ITenantRepository _tenantRepository;
    private readonly TenantMapper _mapper;

    public GetFilteredTenantsQueryHandler(ITenantRepository tenantRepository, TenantMapper mapper)
    {
        _tenantRepository = tenantRepository;
        _mapper = mapper;
    }

    public async Task<List<TenantResponseDTO>> Handle(GetFilteredTenantsQuery request, CancellationToken cancellationToken)
    {
        var tenants = await _tenantRepository.GetFilteredAsync(
            request.CompanyName,
            request.Page, 
            request.PageSize, 
            request.Sort);

        return _mapper.ToTenantResponseDTOList(tenants);
    }
}

using MediatR;
using MultiTenantEcommerce.Application.Platform.Tenancy.Tenants.Common.DTOs;
using MultiTenantEcommerce.Application.Platform.Tenancy.Tenants.Common.Mappers;
using MultiTenantEcommerce.Domain.Platform.Tenancy.Interfaces.Repositories;

namespace MultiTenantEcommerce.Application.Platform.Tenancy.Tenants.Queries.GetFiltered;

public class GetFilteredTenantsQueryHandler : IRequestHandler<GetFilteredTenantsQuery, List<TenantResponseDTO>>
{
    private readonly TenantMapper _mapper;
    private readonly ITenantRepository _tenantRepository;

    public GetFilteredTenantsQueryHandler(ITenantRepository tenantRepository, TenantMapper mapper)
    {
        _tenantRepository = tenantRepository;
        _mapper = mapper;
    }

    public async Task<List<TenantResponseDTO>> Handle(GetFilteredTenantsQuery request,
        CancellationToken cancellationToken)
    {
        var tenants = await _tenantRepository.GetFilteredAsync(
            request.CompanyName,
            request.Page,
            request.PageSize,
            request.Sort);

        return _mapper.ToTenantResponseDTOList(tenants);
    }
}
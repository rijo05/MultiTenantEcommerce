using MediatR;
using MultiTenantEcommerce.Application.Platform.Tenancy.Tenants.Common.DTOs;
using MultiTenantEcommerce.Application.Platform.Tenancy.Tenants.Common.Mappers;
using MultiTenantEcommerce.Domain.Platform.Tenancy.Interfaces.Repositories;

namespace MultiTenantEcommerce.Application.Platform.Tenancy.Tenants.Queries.GetTenantById;

public class GetTenantByIdQueryHandler : IRequestHandler<GetTenantByIdQuery, TenantResponseDTO>
{
    private readonly TenantMapper _mapper;
    private readonly ITenantRepository _tenantRepository;

    public GetTenantByIdQueryHandler(ITenantRepository tenantRepository, TenantMapper mapper)
    {
        _tenantRepository = tenantRepository;
        _mapper = mapper;
    }

    public async Task<TenantResponseDTO> Handle(GetTenantByIdQuery request, CancellationToken cancellationToken)
    {
        var tenant = await _tenantRepository.GetByIdAsync(request.Id)
                     ?? throw new Exception("Tenant doesn't exist.");

        return _mapper.ToTenantResponseDTO(tenant);
    }
}
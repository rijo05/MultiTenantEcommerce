using MediatR;
using MultiTenantEcommerce.Application.Tenants.DTOs.Tenant;
using MultiTenantEcommerce.Application.Tenants.Mappers;
using MultiTenantEcommerce.Domain.Tenants.Interfaces;


namespace MultiTenantEcommerce.Application.Tenants.Queries.Tenant.GetById;
public class GetTenantByIdQueryHandler : IRequestHandler<GetTenantByIdQuery, TenantResponseDTO>
{
    private readonly ITenantRepository _tenantRepository;
    private readonly TenantMapper _mapper;

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

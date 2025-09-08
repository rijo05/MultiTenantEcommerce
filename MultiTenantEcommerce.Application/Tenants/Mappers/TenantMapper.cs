using MultiTenantEcommerce.Application.Common.Helpers;
using MultiTenantEcommerce.Application.Tenants.DTOs.Tenant;
using MultiTenantEcommerce.Domain.Tenants.Entities;

namespace MultiTenantEcommerce.Application.Tenants.Mappers;

public class TenantMapper
{
    private readonly HateoasLinkService _hateoasLinkService;

    public TenantMapper(HateoasLinkService hateoasLinkService)
    {
        _hateoasLinkService = hateoasLinkService;
    }

    public TenantResponseDTO ToTenantResponseDTO(Tenant tenant)
    {
        return new TenantResponseDTO
        {
            CompanyName = tenant.Name,
            CreatedAt = tenant.CreatedAt,
            TenantId = tenant.Id,
            UpdatedAt = tenant.UpdatedAt
        };
    }

    public List<TenantResponseDTO> ToTenantResponseDTOList(IEnumerable<Tenant> tenants)
    {
        return tenants.Select(x => ToTenantResponseDTO(x)).ToList();
    }
}

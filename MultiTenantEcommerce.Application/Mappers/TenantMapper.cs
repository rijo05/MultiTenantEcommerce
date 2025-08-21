using MultiTenantEcommerce.Application.DTOs.Product;
using MultiTenantEcommerce.Application.DTOs.Tenant;
using MultiTenantEcommerce.Application.Services;
using MultiTenantEcommerce.Domain.Entities;

namespace MultiTenantEcommerce.Application.Mappers;

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
            UpdatedAt = tenant.UpdateAt       
        };
    }

    public List<TenantResponseDTO> ToTenantResponseDTOList(List<Tenant> tenants)
    {
        return tenants.Select(x => ToTenantResponseDTO(x)).ToList();
    }
}

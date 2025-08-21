using MultiTenantEcommerce.Application.DTOs.Product;
using MultiTenantEcommerce.Application.DTOs.Tenant;

namespace MultiTenantEcommerce.Application.Interfaces;
public interface ITenantService
{
    public Task<TenantResponseDTO?> GetTenantByIdAsync(Guid id);
    public Task<List<TenantResponseDTO>> GetFilteredTenantsAsync(TenantFilterDTO tenantFilterDTO);

    public Task<TenantResponseDTO> AddTenantAsync(CreateTenantDTO tenant);
    public Task DeleteTenantAsync(Guid id);
    public Task<TenantResponseDTO> UpdateTenantAsync(Guid id, UpdateTenantDTO updatedTenant);
}

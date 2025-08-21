namespace MultiTenantEcommerce.Application.DTOs.Tenant;
public class TenantResponseDTO
{
    public Guid TenantId { get; set; }
    public string CompanyName { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

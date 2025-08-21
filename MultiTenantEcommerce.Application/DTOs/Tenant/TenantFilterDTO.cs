using MultiTenantEcommerce.Domain.Enums;

namespace MultiTenantEcommerce.Application.DTOs.Tenant;
public class TenantFilterDTO
{
    public string? CompanyName { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public SortOptions? Sort { get; set; }
}

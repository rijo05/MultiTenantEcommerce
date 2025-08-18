using MultiTenantEcommerce.Domain.Enums;

namespace MultiTenantEcommerce.Application.DTOs.Employees;

public class EmployeeFilterDTO
{
    public string? Name { get; set; }
    public string? Role { get; set; }
    public string? Email { get; set; }
    public bool? IsActive { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public SortOptions? Sort { get; set; }
}

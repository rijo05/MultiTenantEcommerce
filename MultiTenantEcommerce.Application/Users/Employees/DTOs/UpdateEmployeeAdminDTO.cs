namespace MultiTenantEcommerce.Application.Users.Employees.DTOs;

public record UpdateEmployeeAdminDTO(
    string? Name,
    string? Email,
    string? Password,
    string? Role,
    bool? IsActive);

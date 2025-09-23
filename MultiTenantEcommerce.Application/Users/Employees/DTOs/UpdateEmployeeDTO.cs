namespace MultiTenantEcommerce.Application.Users.Employees.DTOs;
public record UpdateEmployeeDTO(
    string? Name,
    string? Email,
    string? Password);

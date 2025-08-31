namespace MultiTenantEcommerce.Application.Users.Employees.DTOs;

public class EmployeeResponseDTO
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string Email { get; init; }
    public string Role { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
    public bool IsActive { get; init; }
    public Dictionary<string, object> Links { get; init; }
}

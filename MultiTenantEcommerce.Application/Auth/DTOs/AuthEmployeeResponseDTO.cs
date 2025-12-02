namespace MultiTenantEcommerce.Application.Auth.DTOs;
public class AuthEmployeeResponseDTO
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string Email { get; init; }
    public List<string> Roles { get; init; }
    public List<string> Permissions { get; set; }
    public string Token { get; set; }
}

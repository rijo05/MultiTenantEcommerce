namespace MultiTenantEcommerce.Application.Auth.DTOs;
public class AuthEmployeeResponseDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Token { get; set; }
    public List<string> Roles { get; set; }
    public List<string> Permissions { get; set; }
}

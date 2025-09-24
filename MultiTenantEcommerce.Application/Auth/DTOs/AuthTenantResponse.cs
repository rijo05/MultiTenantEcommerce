namespace MultiTenantEcommerce.Application.Auth.DTOs;
public class AuthTenantResponse
{
    public Guid OwnerId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Token { get; set; }
}

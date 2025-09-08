namespace MultiTenantEcommerce.Application.Users.Permissions.DTOs;
public class RoleResponseDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<PermissionResponseDTO> Permissions { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

}

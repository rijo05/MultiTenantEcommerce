namespace MultiTenantEcommerce.Application.DTOs.Order;

public class ChangeOrderStatusDTO
{
    public string Status { get; set; }
    public string? StatusMessage { get; set; }
}

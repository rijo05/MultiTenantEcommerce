using MultiTenantEcommerce.Domain.Enums;

namespace MultiTenantEcommerce.Application.DTOs.Order;

public class OrderFilterDTO
{
    public Guid? customerId { get; set; }
    public string? status { get; set; }
    public DateTime? minDate { get; set; }
    public DateTime? maxDate { get; set; }
    public bool? isPaid { get; set; }
    public decimal? minPrice { get; set; }
    public decimal? maxPrice { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public SortOptions? Sort { get; set; }
}

using MultiTenantEcommerce.Application.DTOs.Order;
using MultiTenantEcommerce.Application.DTOs.OrderItems;

namespace MultiTenantEcommerce.Application.Interfaces;

public interface IOrderService
{
    public Task<OrderResponseDTO> GetOrderByIdAsync(Guid id);
    public Task<List<OrderItemResponseDTO>> GetOrderItemsAsync(Guid id);
    public Task<List<MultipleOrderResponseDTO>> GetFilteredOrdersAsync(OrderFilterDTO order);
    public Task<OrderResponseDTO> CreateOrderAsync(CreateOrderDTO orderDTO);
    public Task<OrderResponseDTO> ChangeOrderStatus(Guid orderId, ChangeOrderStatusDTO statusDTO);
}

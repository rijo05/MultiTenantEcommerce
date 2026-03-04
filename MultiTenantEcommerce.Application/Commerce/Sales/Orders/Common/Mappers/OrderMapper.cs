using MultiTenantEcommerce.Application.Commerce.Sales.Orders.Common.DTOs;
using MultiTenantEcommerce.Application.Commerce.Sales.Orders.Queries.GetByIdAdmin;
using MultiTenantEcommerce.Application.Commerce.Sales.Orders.Queries.GetFilteredAdmin;
using MultiTenantEcommerce.Application.Commerce.Sales.Orders.Queries.GetMyOrderById;
using MultiTenantEcommerce.Application.Commerce.Sales.Orders.Queries.GetMyOrders;
using MultiTenantEcommerce.Domain.Commerce.Sales.Orders.Entities;
using MultiTenantEcommerce.Domain.Commerce.Sales.Orders.ValueObjects;
using MultiTenantEcommerce.Shared.Application;

namespace MultiTenantEcommerce.Application.Commerce.Sales.Orders.Common.Mappers;

public static class OrderMapper
{
    public static OrderAddressDTO ToDTO(this AddressSnapshot address)
    {
        return new OrderAddressDTO(
            address.Street,
            address.City,
            address.PostalCode,
            address.Country,
            address.HouseNumber);
    }

    public static OrderItemDTO ToDTO(this OrderItem item)
    {
        return new OrderItemDTO(
            item.ProductId,
            item.ProductName,
            item.UnitPrice.Value,
            item.Quantity.Value,
            item.Total.Value);
    }

    public static List<OrderItemDTO> ToDTOList(this IEnumerable<OrderItem> items)
        => items.Select(x => x.ToDTO()).ToList();

    public static OrderSummaryDTO ToSummaryDTO(this Order order)
    {
        return new OrderSummaryDTO(
            order.Id,
            order.CreatedAt,
            order.OrderStatus.ToString(),
            order.TotalPrice.Value,
            order.Items.Sum(i => i.Quantity.Value));
    }

    public static PaginatedList<OrderSummaryDTO> ToPaginatedSummaryDTO(this PaginatedList<Order> paginatedOrders)
    {
        var dtoList = paginatedOrders.Items.Select(o => o.ToSummaryDTO()).ToList();
        return new PaginatedList<OrderSummaryDTO>(dtoList, paginatedOrders.TotalCount, paginatedOrders.Page, paginatedOrders.PageSize);
    }

    public static OrderDetailDTO ToDetailDTO(this Order order)
    {
        return new OrderDetailDTO(
            order.Id,
            order.CreatedAt,
            order.ExpiresAt,
            order.OrderStatus.ToString(),
            order.SubTotal.Value,
            order.ShippingCost.Value,
            order.TotalPrice.Value,
            order.Address.ToDTO(),
            order.Items.ToDTOList(),
            order.ToCustomerPaymentSummaryDTO()
        );
    }

    private static PaymentSummaryCustomerDTO ToCustomerPaymentSummaryDTO(this Order order)
    {
        if (order.Payment == null)
        {
            return new PaymentSummaryCustomerDTO("NoPaymentAttempted", null);
        }

        return new PaymentSummaryCustomerDTO(
                    order.Payment.Status.ToString(),
                    order.Payment.FailureReason);
    }

    public static OrderSummaryAdminDTO ToAdminSummaryDTO(this Order order, string customerName)
    {
        var paymentStatusText = order.Payment != null
            ? order.Payment.Status.ToString()
            : "NoPaymentAttempted";

        return new OrderSummaryAdminDTO(
            order.Id,
            order.CustomerId,
            customerName,
            order.CreatedAt,
            order.OrderStatus.ToString(),
            order.TotalPrice.Value,
            paymentStatusText);
    }

    public static PaginatedList<OrderSummaryAdminDTO> ToAdminPaginatedSummaryDTO(
        this PaginatedList<Order> paginatedOrders,
        Dictionary<Guid, string> customerNames)
    {
        var dtoList = paginatedOrders.Items.Select(o =>
                    o.ToAdminSummaryDTO(
                        customerNames.GetValueOrDefault(o.CustomerId, "Unknown Customer"))).ToList();

        return new PaginatedList<OrderSummaryAdminDTO>(dtoList, paginatedOrders.TotalCount, paginatedOrders.Page, paginatedOrders.PageSize);
    }

    public static OrderDetailAdminDTO ToAdminDetailDTO(this Order order, string customerEmail)
    {
        return new OrderDetailAdminDTO(
            order.Id,
            order.CustomerId,
            customerEmail,
            order.CreatedAt,
            order.OrderStatus.ToString(),
            order.SubTotal.Value,
            order.ShippingCost.Value,
            order.TotalPrice.Value,
            order.Address.ToDTO(),
            order.Items.ToDTOList(),
            order.Payment?.ToAdminPaymentAttemptDTO());
    }

    private static PaymentAttemptAdminDTO ToAdminPaymentAttemptDTO(this OrderPayment payment)
    {
        return new PaymentAttemptAdminDTO(
            payment.Id,
            payment.CreatedAt,
            payment.Status.ToString(),
            payment.StripeSessionId,
            payment.TransactionId,
            payment.FailureReason,
            payment.CompletedAt);
    }
}
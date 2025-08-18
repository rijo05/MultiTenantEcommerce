using FluentValidation;
using MultiTenantEcommerce.Application.DTOs.Order;
using MultiTenantEcommerce.Application.DTOs.OrderItems;
using MultiTenantEcommerce.Application.Interfaces;
using MultiTenantEcommerce.Application.Mappers;
using MultiTenantEcommerce.Domain.Entities;
using MultiTenantEcommerce.Domain.Enums;
using MultiTenantEcommerce.Domain.ValueObjects;
using MultiTenantEcommerce.Domain.Interfaces;
using MultiTenantEcommerce.Infrastructure.Context;
using System.Net.Quic;

namespace MultiTenantEcommerce.Application.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly OrderMapper _orderMapper;
    private readonly OrderItemMapper _orderItemMapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateOrderDTO> _validatorCreate;
    private readonly TenantContext _tenantContext;

    public OrderService(IOrderRepository orderRepository, 
        IProductRepository productRepository, 
        ICustomerRepository customerRepository,
        OrderMapper orderMapper, 
        OrderItemMapper orderItemMapper,  
        IUnitOfWork unitOfWork, 
        IValidator<CreateOrderDTO> orderValidator, 
        TenantContext tenantContext)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _customerRepository = customerRepository;
        _orderMapper = orderMapper;
        _orderItemMapper = orderItemMapper;
        _unitOfWork = unitOfWork;
        _validatorCreate = orderValidator;
        _tenantContext = tenantContext;
    }

    #region GETs

    public async Task<List<OrderResponseDTO>> GetFilteredOrdersAsync(OrderFilterDTO order)
    {
        var orders = await _orderRepository.GetFilteredAsync(order.customerId,
            order.status,
            order.minDate,
            order.maxDate,
            order.isPaid,
            order.minPrice,
            order.maxPrice,
            order.Page,
            order.PageSize,
            order.Sort);

        return _orderMapper.ToOrderResponseDTOList(orders);
    }

    public async Task<OrderResponseDTO> GetOrderByIdAsync(Guid id)
    {
        var order = await _orderRepository.GetByIdAsync(id);

        if (order is null)
            throw new Exception("Order not found");

        return _orderMapper.ToOrderResponseDTO(order);
    }

    public async Task<List<OrderItemResponseDTO>> GetOrderItemsAsync(Guid id)
    {
        if (!await _orderRepository.ExistsAsync(id))
            throw new Exception("Order doesnt exist");

        var orderItems = await _orderRepository.GetItemsByOrderIdAsync(id);

        return _orderItemMapper.ToOrderItemResponseDTOList(orderItems);
    }

    #endregion

    #region CHANGE ORDER STATUS

    public async Task<OrderResponseDTO> ChangeOrderStatus(Guid orderId, ChangeOrderStatusDTO statusDTO)
    {
        var order = await EnsureOrderExists(orderId);

        if (!Enum.TryParse<OrderStatus>(statusDTO.Status, true, out var newStatus))
            throw new Exception("Invalid order status");

        order.ChangeStatus(newStatus);
        await _unitOfWork.CommitAsync();
        return _orderMapper.ToOrderResponseDTO(order);
    }

    #endregion

    #region CREATE ORDER

    public async Task<OrderResponseDTO> CreateOrderAsync(CreateOrderDTO orderDTO)
    {
        var validationResults = await _validatorCreate.ValidateAsync(orderDTO);
        if (!validationResults.IsValid)
            throw new ValidationException(validationResults.Errors);

        if (!await _customerRepository.ExistsAsync(orderDTO.CustomerId))
            throw new Exception("Customer doesnt exist");


        var productIds = orderDTO.Items.Select(x => x.Id).ToList();
        var products = await _productRepository.GetByIdsAsync(productIds);

        if (products.Count != productIds.Count)
            throw new Exception("Some products are invalid or do not exist.");


        var orderId = Guid.NewGuid();
        List<OrderItem> itemList = new();

        //VER COMO FAZER STOCK TODO() #####################################################
        foreach (var product in products)
        {
            var quantity = orderDTO.Items.First(x => x.Id == product.Id).Quantity;

            //if (product.StockAvailableAtMoment < quantity)
            //    throw new Exception($"Not enough stock for product {product.Name}.");

            itemList.Add(new OrderItem(orderId, _tenantContext.TenantId, product, quantity));
        }

        var order = new Order(orderId, _tenantContext.TenantId, orderDTO.CustomerId, 
            new Address(orderDTO.Address.Street, 
            orderDTO.Address.City, 
            orderDTO.Address.PostalCode, 
            orderDTO.Address.Country, 
            orderDTO.Address.HouseNumber), 
            itemList, orderDTO.PaymentMethod);
        
        //DISPARAR EVENTOS DE RESERVAR STOCK ###############################

        await _orderRepository.AddAsync(order);
        await _unitOfWork.CommitAsync();

        return _orderMapper.ToOrderResponseDTO(order);
    }


    #endregion


    #region private
    private async Task<Order?> EnsureOrderExists(Guid id)
    {
        return await _orderRepository.GetByIdAsync(id) ?? throw new Exception("Order doesn't exist.");
    }
    #endregion
}

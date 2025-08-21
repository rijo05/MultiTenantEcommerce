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
using System.Transactions;
using Microsoft.EntityFrameworkCore;

namespace MultiTenantEcommerce.Application.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IOrderItemRepository _orderItemRepository;
    private readonly IStockRepository _stockRepository;
    private readonly OrderMapper _orderMapper;
    private readonly OrderItemMapper _orderItemMapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateOrderDTO> _validatorCreate;
    private readonly TenantContext _tenantContext;
    private readonly AppDbContext _appDbContext;

    public OrderService(IOrderRepository orderRepository, 
        IProductRepository productRepository, 
        ICustomerRepository customerRepository,
        IOrderItemRepository orderItemRepository,
        IStockRepository stockRepository,
        OrderMapper orderMapper, 
        OrderItemMapper orderItemMapper,  
        IUnitOfWork unitOfWork, 
        IValidator<CreateOrderDTO> orderValidator, 
        TenantContext tenantContext,
        AppDbContext appDbContext)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _customerRepository = customerRepository;
        _orderItemRepository = orderItemRepository;
        _stockRepository = stockRepository;
        _orderMapper = orderMapper;
        _orderItemMapper = orderItemMapper;
        _unitOfWork = unitOfWork;
        _validatorCreate = orderValidator;
        _tenantContext = tenantContext;
        _appDbContext = appDbContext;
    }

    #region GETs

    public async Task<List<MultipleOrderResponseDTO>> GetFilteredOrdersAsync(OrderFilterDTO order)
    {
        var orders = await _orderRepository.GetFilteredAsync(
            order.customerId,
            order.status,
            order.minDate,
            order.maxDate,
            order.isPaid,
            order.minPrice,
            order.maxPrice,
            order.Page,
            order.PageSize,
            order.Sort);

        return _orderMapper.ToMultipleOrderResponseDTOList(orders);
    }

    public async Task<OrderResponseDTO> GetOrderByIdAsync(Guid id)
    {
        var order = await _orderRepository.GetByIdAsync(id);

        if (order is null)
            throw new Exception("Order not found");

        var items = await GetOrderItemsAsync(id);

        return _orderMapper.ToOrderResponseDTO(order, items);
    }

    public async Task<List<OrderItemResponseDTO>> GetOrderItemsAsync(Guid id)
    {
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
        var items = await GetOrderItemsAsync(orderId);

        await _unitOfWork.CommitAsync();
        return _orderMapper.ToOrderResponseDTO(order, items);
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
        var itemList = orderDTO.Items.Select(item => new OrderItem(
            orderId, _tenantContext.TenantId, products.First(p => p.Id == item.Id), item.Quantity))
            .ToList();

        var order = new Order(orderId, _tenantContext.TenantId, orderDTO.CustomerId, 
            new Address(orderDTO.Address.Street, 
            orderDTO.Address.City, 
            orderDTO.Address.PostalCode, 
            orderDTO.Address.Country, 
            orderDTO.Address.HouseNumber), 
            itemList, orderDTO.PaymentMethod);

        var stockedReserved = await TryReserveStockWithRetries(productIds, itemList, order);

        if (!stockedReserved)
            throw new Exception("Fail to process stock. Please try again in a couple of seconds");

        await _unitOfWork.CommitAsync();

        var items = _orderItemMapper.ToOrderItemResponseDTOList(itemList);

        return _orderMapper.ToOrderResponseDTO(order, items);
    }


    #endregion


    #region private
    private async Task<Order?> EnsureOrderExists(Guid id)
    {
        return await _orderRepository.GetByIdAsync(id) ?? throw new Exception("Order doesn't exist.");
    }

    private async Task ProcessStock(List<Guid> productIds, List<OrderItem> itens, Order order)
    {
        var stocks = await _stockRepository.GetBulkByIdsAsync(productIds);

        if (stocks.Count != itens.Count)
            throw new Exception("error");

        itens = itens.OrderBy(x => x.ProductId).ToList();
        stocks = stocks.OrderBy(x => x.ProductId).ToList();

        foreach (var item in itens)
        {
            var stock = stocks.FirstOrDefault(s => s.ProductId == item.ProductId);
            if (stock == null || item.Quantity > stock.StockAvailableAtMoment)
                throw new Exception($"Not enough stock of product {item.Name}");

            stock.ReserveStock(item.Quantity);
        }
        await _orderRepository.AddAsync(order);
        await _orderItemRepository.AddBulkAsync(itens);


        foreach (var stock in stocks)
        {
            try
            {
                await _stockRepository.SaveAsync(stock);
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new Exception($"Concurrency conflict while reserving stock for product {stock.ProductId}");
            }
        }



        await _appDbContext.SaveChangesAsync();
    }

    private async Task<bool> TryReserveStockWithRetries(List<Guid> productIds, List<OrderItem> itens, Order order)
    {
        bool success = false;
        int retries = 3;
        int delayBetweenRetries = 1000;

        for (int attempt = 0; attempt < retries; attempt++)
        {
            try
            {
                await ProcessStock(productIds, itens, order);
                success = true;
                break;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (attempt == retries - 1)
                    throw;
                await Task.Delay(delayBetweenRetries);
            }
        }

        return success;
    }
    #endregion
}

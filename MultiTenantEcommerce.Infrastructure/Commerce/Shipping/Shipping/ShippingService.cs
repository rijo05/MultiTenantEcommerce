using MultiTenantEcommerce.Application.Commerce.Shipping.Common.DTOs;
using MultiTenantEcommerce.Application.Commerce.Shipping.Interfaces;
using MultiTenantEcommerce.Application.Commerce.Shipping.Services;
using MultiTenantEcommerce.Application.Common.Interfaces.Persistence;
using MultiTenantEcommerce.Domain.Commerce.Customers.Interfaces;
using MultiTenantEcommerce.Domain.Commerce.Sales.Orders.Entities;
using MultiTenantEcommerce.Domain.Commerce.Sales.Orders.Enums;
using MultiTenantEcommerce.Domain.Commerce.Shipping.Entities;
using MultiTenantEcommerce.Domain.Commerce.Shipping.Enums;
using MultiTenantEcommerce.Domain.Commerce.Shipping.Interfaces;
using MultiTenantEcommerce.Domain.Platform.Tenancy.Interfaces.Repositories;
using MultiTenantEcommerce.Shared.Application;

namespace MultiTenantEcommerce.Infrastructure.Commerce.Shipping.Shipping;

public class ShippingService : IShippingService
{
    private readonly IAddressValidator _addressValidator;
    private readonly ICustomerRepository _customerRepository;
    private readonly IShippingProviderFactory _factory;
    private readonly IFileStorageService _fileStorageService;
    private readonly IShipmentRepository _shipmentRepository;
    private readonly ITenantContext _tenantContext;
    private readonly ITenantRepository _tenantRepository;

    public ShippingService(IShippingProviderFactory factory,
        ITenantRepository tenantRepository,
        ITenantContext tenantContext,
        ICustomerRepository customerRepository,
        IAddressValidator addressValidator,
        IFileStorageService fileStorageService,
        IShipmentRepository shipmentRepository)
    {
        _factory = factory;
        _tenantRepository = tenantRepository;
        _tenantContext = tenantContext;
        _customerRepository = customerRepository;
        _addressValidator = addressValidator;
        _fileStorageService = fileStorageService;
        _shipmentRepository = shipmentRepository;
    }

    public async Task<Shipment> CreateShipment(Guid tenantId, Order order, ShipmentCarrier carrier)
    {
        if (order.OrderStatus != OrderStatus.Processing)
            throw new Exception("Cant create a shipment for a order not in processing");

        if (await _shipmentRepository.GetByOrderId(order.Id) != null)
            throw new Exception("This order already has a shipment");

        var customer = await _customerRepository.GetByIdAsync(order.CustomerId)
                       ?? throw new Exception("Customer doesnt exist");

        var provider = _factory.GetProvider(carrier);

        var info = await provider.CreateShipment(order, customer.Name)
                   ?? throw new Exception("couldnt create shipment");

        var shipment = new Shipment(order.TenantId,
            order.Id,
            info.TrackingNumber,
            order.Address,
            carrier,
            new Money(info.price),
            info.MinTransit,
            info.MaxTransit);

        await _fileStorageService.UploadAsync(shipment.LabelKey, info.LabelPdf, MimeTypes.Pdf);

        return shipment;
    }

    public async Task<ShippingQuoteDTO> GetShippingQuote(ShipmentCarrier carrier, string address)
    {
        var acceptableShippingProviders = await _tenantRepository.GetByIdAsync(_tenantContext.TenantId) ??
                                  throw new Exception("Tenant not found");

        var checkCarrier = acceptableShippingProviders.ShippingProviders.Where(x => x.IsActive == true && x.carrier = carrier);

        if (checkCarrier == null)
            throw new Exception("Cant use this carries");

        var provider = _factory.GetProvider(carrier);
        var quote = await provider.GetQuote(address);

        return quote;
    }

    public async Task<List<ShippingQuoteDTO>> GetShippingQuotes(Address address)
    {
        var acceptableShippingProviders = await _tenantRepository.GetByIdAsync(_tenantContext.TenantId) ??
                                          throw new Exception("Tenant not found");

        //CACHE FUTURO ################

        var result = await _addressValidator.IsAddressValid(address);

        if (!result.IsValid)
            throw new Exception("Address not valid");


        var quotes = new List<ShippingQuoteDTO>();

        foreach (var item in acceptableShippingProviders.ShippingProviders.Where(x => x.IsActive == true))
        {
            var provider = _factory.GetProvider(item.Carrier);
            var quote = await provider.GetQuote(address);

            quotes.Add(quote);
        }

        return quotes;
    }
}
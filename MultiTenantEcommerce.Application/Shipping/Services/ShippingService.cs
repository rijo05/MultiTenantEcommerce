using MultiTenantEcommerce.Application.Common.Helpers;
using MultiTenantEcommerce.Application.Common.Interfaces.Persistence;
using MultiTenantEcommerce.Application.Shipping.DTOs;
using MultiTenantEcommerce.Application.Shipping.Interfaces;
using MultiTenantEcommerce.Domain.Sales.Orders.Entities;
using MultiTenantEcommerce.Domain.Shipping.Entities;
using MultiTenantEcommerce.Domain.Shipping.Enums;
using MultiTenantEcommerce.Domain.Shipping.Interfaces;
using MultiTenantEcommerce.Domain.Tenants.Interfaces;
using MultiTenantEcommerce.Domain.Users.Interfaces;
using MultiTenantEcommerce.Domain.ValueObjects;

namespace MultiTenantEcommerce.Application.Shipping.Services;
public class ShippingService : IShippingService
{
    private readonly IShippingProviderFactory _factory;
    private readonly ITenantRepository _tenantRepository;
    private readonly ITenantContext _tenantContext;
    private readonly ICustomerRepository _customerRepository;
    private readonly IAddressValidator _addressValidator;
    private readonly IFileStorageService _fileStorageService;
    private readonly IShipmentRepository _shipmentRepository;

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
        if (order.OrderStatus != Domain.Enums.OrderStatus.Processing)
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

    public async Task<List<ShippingQuoteDTO>> GetShippingQuotes(Address address)
    {
        var acceptableShippingProviders = await _tenantRepository.GetByIdAsync(_tenantContext.TenantId) ??
            throw new Exception("Tenant not found");

        //CACHE FUTURO ################

        var result = await _addressValidator.IsAddressValid(address);

        if (!result.IsValid)
            throw new Exception("Address not valid");


        List<ShippingQuoteDTO> quotes = new List<ShippingQuoteDTO>();

        foreach (var item in acceptableShippingProviders.ShippingProviders.Where(x => x.IsActive == true))
        {
            var provider = _factory.GetProvider(item.Carrier);
            var quote = await provider.GetQuote(address);

            quotes.Add(quote);
        }

        return quotes;
    }
}

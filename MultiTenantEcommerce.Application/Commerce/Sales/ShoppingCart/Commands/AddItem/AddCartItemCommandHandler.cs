using MultiTenantEcommerce.Application.Commerce.Sales.ShoppingCart.Common.DTOs;
using MultiTenantEcommerce.Application.Commerce.Sales.ShoppingCart.Common.Mappers;
using MultiTenantEcommerce.Application.Common.Interfaces.Services;
using MultiTenantEcommerce.Domain.Commerce.Catalog.Interfaces;
using MultiTenantEcommerce.Domain.Commerce.Sales.ShoppingCart.Entities;
using MultiTenantEcommerce.Domain.Commerce.Sales.ShoppingCart.Interfaces;
using MultiTenantEcommerce.Domain.Logistics.Inventory.Interfaces;
using MultiTenantEcommerce.Shared.Application.CQRS;
using MultiTenantEcommerce.Shared.Domain.ValueObjects;
using MultiTenantEcommerce.Shared.Infrastructure.Persistence;
using MultiTenantEcommerce.Shared.Integration.Proxies;
using System.Data.Entity.Infrastructure;

namespace MultiTenantEcommerce.Application.Commerce.Sales.ShoppingCart.Commands.AddItem;

public class AddCartItemCommandHandler : ICommandHandler<AddCartItemCommand, CartSummaryDTO>
{
    private readonly ICartRepository _cartRepository;
    private readonly ICatalogIntegrationProxy _catalogIntegrationProxy;
    private readonly ITenantContext _tenantContext;
    private readonly IUnitOfWork _unitOfWork;

    public AddCartItemCommandHandler(ICartRepository cartRepository,
        ICatalogIntegrationProxy catalogIntegrationProxy,
        IUnitOfWork unitOfWork,
        ITenantContext tenantContext)
    {
        _cartRepository = cartRepository;
        _catalogIntegrationProxy = catalogIntegrationProxy;
        _unitOfWork = unitOfWork;
        _tenantContext = tenantContext;
    }

    public async Task<CartSummaryDTO> Handle(AddCartItemCommand request, CancellationToken cancellationToken)
    {
        var cart = await _cartRepository.GetActiveCartAsync(request.CustomerId, request.AnonymousId);

        if (cart == null)
        {
            cart = new Cart(_tenantContext.TenantId, request.CustomerId, request.AnonymousId);
            await _cartRepository.AddAsync(cart);
        }

        var product = await _catalogIntegrationProxy.GetProductById(request.ProductId)
                      ?? throw new Exception("Product doesnt exist");

        if (product.StockStatus == "OutOfStock")
            throw new Exception("No stock");

        cart.AddItem(product.Id, new PositiveQuantity(request.Quantity));

        await _unitOfWork.CommitAsync();

        return cart.ToSummaryDTO();
    }
}
using MultiTenantEcommerce.Application.Commerce.Sales.ShoppingCart.Common.DTOs;
using MultiTenantEcommerce.Application.Commerce.Sales.ShoppingCart.Common.Mappers;
using MultiTenantEcommerce.Domain.Commerce.Catalog.Interfaces;
using MultiTenantEcommerce.Domain.Commerce.Sales.ShoppingCart.Interfaces;
using MultiTenantEcommerce.Domain.Logistics.Inventory.Interfaces;
using MultiTenantEcommerce.Shared.Application.CQRS;
using MultiTenantEcommerce.Shared.Domain.ValueObjects;
using MultiTenantEcommerce.Shared.Infrastructure.Persistence;
using MultiTenantEcommerce.Shared.Integration.Proxies;

namespace MultiTenantEcommerce.Application.Commerce.Sales.ShoppingCart.Commands.DecreaseItem;

public class DecreaseCartItemCommandHandler : ICommandHandler<DecreaseCartItemCommand, CartSummaryDTO>
{
    private readonly ICartRepository _cartRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DecreaseCartItemCommandHandler(ICartRepository cartRepository,
        IUnitOfWork unitOfWork)
    {
        _cartRepository = cartRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CartSummaryDTO> Handle(DecreaseCartItemCommand request, CancellationToken cancellationToken)
    {
        var cart = await _cartRepository.GetActiveCartAsync(request.CustomerId, request.AnonymousId) 
            ?? throw new Exception("cart doesnt exist");

        cart.DecreaseItem(request.ProductId, new PositiveQuantity(request.Quantity));
        
        await _unitOfWork.CommitAsync();

        return cart.ToSummaryDTO();
    }
}
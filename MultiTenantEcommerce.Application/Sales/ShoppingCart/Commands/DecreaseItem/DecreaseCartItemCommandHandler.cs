using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Common.Interfaces.Persistence;
using MultiTenantEcommerce.Application.Sales.ShoppingCart.DTOs;
using MultiTenantEcommerce.Application.Sales.ShoppingCart.Mappers;
using MultiTenantEcommerce.Domain.Sales.ShoppingCart.Interfaces;
using MultiTenantEcommerce.Domain.ValueObjects;

namespace MultiTenantEcommerce.Application.Sales.ShoppingCart.Commands.RemoveItem;
public class DecreaseCartItemCommandHandler : ICommandHandler<DecreaseCartItemCommand, CartResponseDTO>
{
    private readonly ICartRepository _cartRepository;
    private readonly CartMapper _cartMapper;
    private readonly IUnitOfWork _unitOfWork;

    public DecreaseCartItemCommandHandler(ICartRepository cartRepository,
        CartMapper cartMapper,
        IUnitOfWork unitOfWork)
    {
        _cartRepository = cartRepository;
        _cartMapper = cartMapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<CartResponseDTO> Handle(DecreaseCartItemCommand request, CancellationToken cancellationToken)
    {
        var cart = await _cartRepository.GetByCustomerIdAsync(request.CustomerId)
            ?? throw new Exception("Cart doesnt exist");

        cart.DecreaseItem(request.ProductId, new PositiveQuantity(request.quantity));

        await _unitOfWork.CommitAsync();

        return _cartMapper.ToCartResponseDTO(cart);
    }
}

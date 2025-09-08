using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Common.Interfaces.Persistence;
using MultiTenantEcommerce.Application.Sales.ShoppingCart.DTOs;
using MultiTenantEcommerce.Application.Sales.ShoppingCart.Mappers;
using MultiTenantEcommerce.Domain.Sales.ShoppingCart.Interfaces;

namespace MultiTenantEcommerce.Application.Sales.ShoppingCart.Commands.RemoveItem;
public class RemoveCartItemCommandHandler : ICommandHandler<RemoveCartItemCommand, CartResponseDTO>
{
    private readonly ICartRepository _cartRepository;
    private readonly CartMapper _cartMapper;
    private readonly IUnitOfWork _unitOfWork;

    public RemoveCartItemCommandHandler(ICartRepository cartRepository,
        CartMapper cartMapper,
        IUnitOfWork unitOfWork)
    {
        _cartRepository = cartRepository;
        _cartMapper = cartMapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<CartResponseDTO> Handle(RemoveCartItemCommand request, CancellationToken cancellationToken)
    {
        var cart = await _cartRepository.GetByCustomerIdAsync(request.CustomerId)
            ?? throw new Exception("Cart doesnt exist");

        cart.RemoveItem(request.ProductId);

        await _unitOfWork.CommitAsync();

        return _cartMapper.ToCartResponseDTO(cart);
    }
}

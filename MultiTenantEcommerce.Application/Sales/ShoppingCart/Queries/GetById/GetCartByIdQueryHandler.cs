using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Sales.ShoppingCart.DTOs;
using MultiTenantEcommerce.Application.Sales.ShoppingCart.Mappers;
using MultiTenantEcommerce.Domain.Sales.ShoppingCart.Interfaces;

namespace MultiTenantEcommerce.Application.Sales.ShoppingCart.Queries.GetById;
public class GetCartByIdQueryHandler : IQueryHandler<GetCartByIdQuery, CartResponseDTO>
{
    private readonly ICartRepository _cartRepository;
    private readonly CartMapper _cartMapper;

    public GetCartByIdQueryHandler(ICartRepository cartRepository,
        CartMapper cartMapper)
    {
        _cartRepository = cartRepository;
        _cartMapper = cartMapper;
    }

    public async Task<CartResponseDTO> Handle(GetCartByIdQuery request, CancellationToken cancellationToken)
    {
        var cart = await _cartRepository.GetByIdAsync(request.CartId)
            ?? throw new Exception("Cart doesnt exist");

        return _cartMapper.ToCartResponseDTO(cart);
    }
}

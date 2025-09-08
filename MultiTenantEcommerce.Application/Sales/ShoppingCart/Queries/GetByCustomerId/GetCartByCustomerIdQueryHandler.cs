using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Sales.ShoppingCart.DTOs;
using MultiTenantEcommerce.Application.Sales.ShoppingCart.Mappers;
using MultiTenantEcommerce.Domain.Sales.ShoppingCart.Interfaces;

namespace MultiTenantEcommerce.Application.Sales.ShoppingCart.Queries.GetByCustomerId;
public class GetCartByCustomerIdQueryHandler : IQueryHandler<GetCartByCustomerIdQuery, CartResponseDTO>
{
    private readonly ICartRepository _cartRepository;
    private readonly CartMapper _cartMapper;

    public GetCartByCustomerIdQueryHandler(ICartRepository cartRepository,
        CartMapper cartMapper)
    {
        _cartRepository = cartRepository;
        _cartMapper = cartMapper;
    }

    public async Task<CartResponseDTO> Handle(GetCartByCustomerIdQuery request, CancellationToken cancellationToken)
    {
        var cart = await _cartRepository.GetByCustomerIdAsync(request.CustomerId)
            ?? throw new Exception("Customer doesnt have an open cart");

        return _cartMapper.ToCartResponseDTO(cart);
    }
}

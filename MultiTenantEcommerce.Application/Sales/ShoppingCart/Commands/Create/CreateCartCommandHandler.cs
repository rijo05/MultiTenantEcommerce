using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Common.Interfaces.Persistence;
using MultiTenantEcommerce.Application.Sales.ShoppingCart.DTOs;
using MultiTenantEcommerce.Application.Sales.ShoppingCart.Mappers;
using MultiTenantEcommerce.Domain.Sales.ShoppingCart.Entities;
using MultiTenantEcommerce.Domain.Sales.ShoppingCart.Interfaces;

namespace MultiTenantEcommerce.Application.Sales.ShoppingCart.Commands.Create;
public class CreateCartCommandHandler : ICommandHandler<CreateCartCommand, CartResponseDTO>
{
    private readonly ICartRepository _cartRepository;
    private readonly CartMapper _cartMapper;
    private readonly ITenantContext _tenantContext;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCartCommandHandler(ICartRepository cartRepository,
        CartMapper cartMapper,
        ITenantContext tenantContext,
        IUnitOfWork unitOfWork)
    {
        _cartRepository = cartRepository;
        _cartMapper = cartMapper;
        _tenantContext = tenantContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<CartResponseDTO> Handle(CreateCartCommand request, CancellationToken cancellationToken)
    {
        var existingCart = await _cartRepository.GetByCustomerIdAsync(request.CustomerId);

        if (existingCart is not null)
            return _cartMapper.ToCartResponseDTO(existingCart);

        var cart = new Cart(_tenantContext.TenantId, request.CustomerId);

        await _cartRepository.AddAsync(cart);
        await _unitOfWork.CommitAsync();

        return _cartMapper.ToCartResponseDTO(cart);
    }
}

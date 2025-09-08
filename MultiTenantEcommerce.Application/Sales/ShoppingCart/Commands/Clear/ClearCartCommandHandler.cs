using MediatR;
using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Common.Interfaces.Persistence;
using MultiTenantEcommerce.Domain.Sales.ShoppingCart.Interfaces;

namespace MultiTenantEcommerce.Application.Sales.ShoppingCart.Commands.Clear;
public class ClearCartCommandHandler : ICommandHandler<ClearCartCommand, Unit>
{
    private readonly ICartRepository _cartRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ClearCartCommandHandler(ICartRepository cartRepository, IUnitOfWork unitOfWork)
    {
        _cartRepository = cartRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(ClearCartCommand request, CancellationToken cancellationToken)
    {
        var cart = await _cartRepository.GetByCustomerIdAsync(request.CustomerId)
            ?? throw new Exception("Cart doesnt exist.");

        cart.ClearCart();

        await _unitOfWork.CommitAsync();

        return Unit.Value;
    }
}

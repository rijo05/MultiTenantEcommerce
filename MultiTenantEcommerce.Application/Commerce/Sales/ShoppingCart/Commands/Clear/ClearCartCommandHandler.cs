using MediatR;
using MultiTenantEcommerce.Domain.Commerce.Sales.ShoppingCart.Interfaces;
using MultiTenantEcommerce.Shared.Application.CQRS;
using MultiTenantEcommerce.Shared.Infrastructure.Persistence;

namespace MultiTenantEcommerce.Application.Commerce.Sales.ShoppingCart.Commands.Clear;

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
        var cart = await _cartRepository.GetActiveCartAsync(request.CustomerId, request.AnonymousId);

        cart?.ClearCart();

        await _unitOfWork.CommitAsync();

        return Unit.Value;
    }
}
using MediatR;
using MultiTenantEcommerce.Domain.Commerce.Sales.ShoppingCart.Interfaces;
using MultiTenantEcommerce.Shared.Application.CQRS;
using MultiTenantEcommerce.Shared.Infrastructure.Persistence;

namespace MultiTenantEcommerce.Application.Commerce.Sales.ShoppingCart.Commands.MergeCart;
public class MergeCartCommandHandler : ICommandHandler<MergeCartCommand, Unit>
{
    private readonly ICartRepository _cartRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITenantContext _tenantContext;

    public MergeCartCommandHandler(ICartRepository cartRepository, IUnitOfWork unitOfWork, ITenantContext tenantContext)
    {
        _cartRepository = cartRepository;
        _unitOfWork = unitOfWork;
        _tenantContext = tenantContext;
    }

    public async Task<Unit> Handle(MergeCartCommand request, CancellationToken ct)
    {
        var anonymousCart = await _cartRepository.GetActiveCartAsync(null, request.AnonymousId);

        if (anonymousCart == null || anonymousCart.IsEmpty())
            return Unit.Value;

        var customerCart = await _cartRepository.GetActiveCartAsync(request.CustomerId, null);

        if (customerCart == null)
        {
            anonymousCart.AssignToCustomer(request.CustomerId);
        }
        else
        {
            foreach (var item in anonymousCart.Items)
            {
                customerCart.AddItem(item.ProductId, item.Quantity);
            }

            anonymousCart.Close();
        }

        await _unitOfWork.CommitAsync();

        return Unit.Value;
    }
}

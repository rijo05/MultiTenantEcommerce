using MediatR;
using MultiTenantEcommerce.Domain.Platform.Tenancy.Interfaces.Repositories;

namespace MultiTenantEcommerce.Application.Commerce.Shipping.Commands.DeactivateCarrier;

public class DeactivateCarrierCommandHandler : ICommandHandler<DeactivateCarrierCommand, Unit>
{
    private readonly ITenantContext _tenantContext;
    private readonly ITenantRepository _tenantRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeactivateCarrierCommandHandler(ITenantRepository tenantRepository,
        ITenantContext tenantContext,
        IUnitOfWork unitOfWork)
    {
        _tenantRepository = tenantRepository;
        _tenantContext = tenantContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(DeactivateCarrierCommand request, CancellationToken cancellationToken)
    {
        var tenant = await _tenantRepository.GetByIdAsync(_tenantContext.TenantId)
                     ?? throw new Exception("Tenant doesnt exist");

        var carrier = tenant.ShippingProviders.FirstOrDefault(x => x.Carrier == request.Carrier)
                      ?? throw new Exception("Carrier doesnt exist");

        tenant.DeactivateCarrier(carrier.Carrier);

        await _unitOfWork.CommitAsync();

        return Unit.Value;
    }
}
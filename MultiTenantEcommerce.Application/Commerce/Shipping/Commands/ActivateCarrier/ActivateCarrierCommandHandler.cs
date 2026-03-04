using MediatR;
using MultiTenantEcommerce.Domain.Platform.Tenancy.Interfaces.Repositories;

namespace MultiTenantEcommerce.Application.Commerce.Shipping.Commands.ActivateCarrier;

public class ActivateCarrierCommandHandler : ICommandHandler<ActivateCarrierCommand, Unit>
{
    private readonly ITenantContext _tenantContext;
    private readonly ITenantRepository _tenantRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ActivateCarrierCommandHandler(ITenantRepository tenantRepository,
        ITenantContext tenantContext,
        IUnitOfWork unitOfWork)
    {
        _tenantRepository = tenantRepository;
        _tenantContext = tenantContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(ActivateCarrierCommand request, CancellationToken cancellationToken)
    {
        var tenant = await _tenantRepository.GetByIdAsync(_tenantContext.TenantId)
                     ?? throw new Exception("Tenant doesnt exist");

        var carrier = tenant.ShippingProviders.FirstOrDefault(x => x.Carrier == request.Carrier)
                      ?? throw new Exception("Carrier doesnt exist");

        tenant.ActivateCarrier(carrier.Carrier);

        await _unitOfWork.CommitAsync();

        return Unit.Value;
    }
}
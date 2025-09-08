using MediatR;
using MultiTenantEcommerce.Application.Common.Interfaces.Persistence;
using MultiTenantEcommerce.Domain.Tenants.Interfaces;

namespace MultiTenantEcommerce.Application.Tenants.Commands.Tenant.Delete;
public class DeleteTenantCommandHandler : IRequestHandler<DeleteTenantCommand, Unit>
{
    private readonly ITenantRepository _tenantRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteTenantCommandHandler(ITenantRepository tenantRepository, IUnitOfWork unitOfWork)
    {
        _tenantRepository = tenantRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(DeleteTenantCommand request, CancellationToken cancellationToken)
    {
        var tenant = await _tenantRepository.GetByIdAsync(request.Id)
            ?? throw new Exception("Tenant doesn't exist.");

        await _tenantRepository.DeleteAsync(tenant);
        await _unitOfWork.CommitAsync();

        return Unit.Value;
    }
}

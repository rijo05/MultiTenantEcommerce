using MediatR;
using MultiTenantEcommerce.Domain.Platform.Tenancy.Interfaces.Repositories;

namespace MultiTenantEcommerce.Application.Platform.Tenancy.TenantMembers.Commands.RemoveMember;

public class DeleteTenantMemberCommandHandler : ICommandHandler<DeleteTenantMemberCommand, Unit>
{
    private readonly ITenantMemberRepository _employeeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteTenantMemberCommandHandler(ITenantMemberRepository employeeRepository,
        IUnitOfWork unitOfWork)
    {
        _employeeRepository = employeeRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(DeleteTenantMemberCommand request, CancellationToken cancellationToken)
    {
        var employee = await _employeeRepository.GetByIdAsync(request.id)
                       ?? throw new Exception("TenantMember doesnt exist.");

        await _employeeRepository.DeleteAsync(employee);

        await _unitOfWork.CommitAsync();
        return Unit.Value;
    }
}
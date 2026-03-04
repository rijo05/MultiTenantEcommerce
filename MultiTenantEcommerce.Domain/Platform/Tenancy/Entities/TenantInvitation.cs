using MultiTenantEcommerce.Shared.Domain.Abstractions;
using MultiTenantEcommerce.Shared.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Domain.Platform.Tenancy.Entities;
public class TenantInvitation : TenantBase
{
    public Email Email {  get; private set; }
    public Guid Token {  get; private set; }
    public DateTime ExpiresAt { get; private set; }
    public bool IsAccepted { get; private set; }
    public IReadOnlyCollection<Guid> Roles => _roles;
    private readonly HashSet<Guid> _roles = new();

    public TenantInvitation(Guid tenantId, Email email, List<Guid> roles) : base(tenantId)
    {
        Email = email;
        Token = Guid.NewGuid();
        _roles = roles.ToHashSet();

        ExpiresAt = DateTime.UtcNow.AddHours(48);
        IsAccepted = false;
    }

    public void AcceptInvite()
    {
        if (IsAccepted)
            throw new Exception("Este convite ja foi utilizado");

        if (ExpiresAt < DateTime.UtcNow)
            throw new Exception("Ja expirou, crie outro");

        IsAccepted = true;
        SetUpdatedAt();
    }

    public void CancelInvite()
    {
        if (IsAccepted)
            throw new Exception("Este convite ja foi aceite");

        ExpiresAt = DateTime.UtcNow;
        SetUpdatedAt();
    }
}

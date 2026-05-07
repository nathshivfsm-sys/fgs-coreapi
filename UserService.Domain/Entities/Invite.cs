using UserService.Domain.Enums;

namespace UserService.Domain.Entities;

public sealed class Invite
{
    public Guid Id { get; private set; }
    public Guid TenantId { get; private set; }
    public Guid UserId { get; private set; }
    public string InvitedEmail { get; private set; } = null!;
    public short CompanyId { get; private set; }
    public byte[] TokenHash { get; private set; } = null!;
    public InviteStatus Status { get; private set; }
    public DateTimeOffset ExpiresAt { get; private set; }
    public DateTimeOffset? AcceptedAt { get; private set; }
    public DateTimeOffset? RevokedAt { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }

    public Tenant Tenant { get; private set; } = null!;
    public User User { get; private set; } = null!;
    public TenantCompany TenantCompany { get; private set; } = null!;

    private Invite()
    {
    }

    public static Invite CreatePending(
        Guid tenantId,
        Guid userId,
        string invitedEmail,
        short companyId,
        byte[] tokenHash,
        DateTimeOffset expiresAt)
    {
        return new Invite
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,
            UserId = userId,
            InvitedEmail = invitedEmail,
            CompanyId = companyId,
            TokenHash = tokenHash,
            Status = InviteStatus.Pending,
            ExpiresAt = expiresAt,
            CreatedAt = DateTimeOffset.UtcNow
        };
    }
}

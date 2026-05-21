using Fgs.User.Domain.Enums;

namespace Fgs.User.Domain.Entities;

/// <summary>
/// Email verification / onboarding invitation (stores hashed token only).
/// </summary>
public class FgsInvitation : FgsEntityBase
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public long TenantId { get; set; }

    public string Email { get; set; } = null!;

    public string TokenHash { get; set; } = null!;

    public InvitationStatus Status { get; set; } = InvitationStatus.Pending;

    public DateTimeOffset ExpiresAtUtc { get; set; }

    public DateTimeOffset? AcceptedAtUtc { get; set; }

    public FgsUser? User { get; set; }

    public bool IsActive =>
        Status == InvitationStatus.Pending && ExpiresAtUtc > DateTimeOffset.UtcNow;

    public void MarkAccepted()
    {
        Status = InvitationStatus.Accepted;
        AcceptedAtUtc = DateTimeOffset.UtcNow;
    }

    public void MarkExpired()
    {
        Status = InvitationStatus.Expired;
    }
}

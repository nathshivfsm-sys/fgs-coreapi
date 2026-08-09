namespace Fgs.User.Application.Abstractions.Invitations;

public enum InvitationEmailKind
{
    CompanyAdminSignup = 1,
    UserInvited = 2
}

public sealed record IssueInvitationRequest(
    Guid UserId,
    long TenantId,
    long CompanyId,
    string Email,
    string DisplayName,
    InvitationEmailKind Kind,
    Guid? InvitationId = null,
    string? CreatedBy = null,
    DateTimeOffset? UtcNow = null,
    bool SupersedePendingForUser = false,
    string? CompanyName = null);

public sealed record IssuedInvitation(
    Guid InvitationId,
    string InviteUrl,
    DateTimeOffset ExpiresAtUtc,
    int ExpirationHours);

public interface IUserInvitationIssuer
{
    /// <summary>
    /// Creates a pending invitation and enqueues the invite email outbox message.
    /// Caller is responsible for persisting (SaveChanges / transaction commit).
    /// </summary>
    Task<IssuedInvitation> IssueAsync(
        IssueInvitationRequest request,
        CancellationToken cancellationToken = default);
}

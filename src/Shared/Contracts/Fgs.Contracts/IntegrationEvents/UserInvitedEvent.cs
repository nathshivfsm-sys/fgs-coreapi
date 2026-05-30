namespace Fgs.Contracts.IntegrationEvents;

public sealed record UserInvitedEvent(
    long TenantId,
    long CompanyId,
    Guid UserId,
    string Email,
    string DisplayName,
    string InviteUrl);

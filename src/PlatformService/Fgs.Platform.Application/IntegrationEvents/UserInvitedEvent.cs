namespace Fgs.Platform.Application.IntegrationEvents;

public sealed record UserInvitedEvent(
    long TenantId,
    string Email,
    string DisplayName,
    string InviteUrl);

namespace Fgs.Platform.Application.IntegrationEvents;

public sealed record UserInvitedEvent(
    Guid TenantId,
    string Email,
    string DisplayName,
    string InviteUrl);

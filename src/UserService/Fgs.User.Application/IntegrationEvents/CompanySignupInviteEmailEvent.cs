namespace Fgs.User.Application.IntegrationEvents;

public sealed record CompanySignupInviteEmailEvent(
    Guid TenantId,
    Guid CompanyId,
    Guid UserId,
    Guid InvitationId,
    string Email,
    string DisplayName,
    string InviteUrl);

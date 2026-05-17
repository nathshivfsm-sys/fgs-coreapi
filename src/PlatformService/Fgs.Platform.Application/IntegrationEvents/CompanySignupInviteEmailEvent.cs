namespace Fgs.Platform.Application.IntegrationEvents;

public sealed record CompanySignupInviteEmailEvent(
    Guid TenantId,
    Guid CompanyId,
    Guid UserId,
    Guid InvitationId,
    string Email,
    string EmailTemplateCode,
    string Name,
    string PlatformName,
    string InviteLink,
    string ExpirationHours,
    string CompanyName,
    string SupportEmail);

namespace Fgs.Contracts.IntegrationEvents;

public sealed record CompanySignupInviteEmailEvent(
    long TenantId,
    long CompanyId,
    Guid UserId,
    Guid InvitationId,
    string Email,
    string EmailTemplateCode,
    string Name,
    string PlatformName,
    string InviteLink,
    string ExpirationHours,
    string SupportEmail);

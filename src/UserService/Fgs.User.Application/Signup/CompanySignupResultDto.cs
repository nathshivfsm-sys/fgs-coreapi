namespace Fgs.User.Application.Signup;

public sealed record CompanySignupResultDto(
    Guid TenantId,
    long CompanyId,
    Guid CompanyUid,
    Guid UserId,
    Guid InvitationId,
    string InviteUrl);

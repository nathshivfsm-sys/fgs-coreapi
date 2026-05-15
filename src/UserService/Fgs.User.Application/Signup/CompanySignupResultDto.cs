namespace Fgs.User.Application.Signup;

public sealed record CompanySignupResultDto(
    Guid TenantId,
    Guid CompanyId,
    Guid UserId,
    Guid InvitationId,
    string InviteUrl);

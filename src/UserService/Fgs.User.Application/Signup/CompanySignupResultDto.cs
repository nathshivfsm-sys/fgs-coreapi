namespace Fgs.User.Application.Signup;

/// <summary>
/// Result of company self-serve signup. <see cref="CompanyNumber"/> is the tenant-scoped company key
/// (same value stored as <c>CompanyId</c> on <c>Fgs*</c> tables).
/// </summary>
public sealed record CompanySignupResultDto(
    Guid TenantId,
    long CompanyNumber,
    Guid CompanyGuid,
    Guid UserId,
    Guid InvitationId,
    string InviteUrl);

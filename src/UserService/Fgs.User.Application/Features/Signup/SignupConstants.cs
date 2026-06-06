namespace Fgs.User.Application.Features.Signup;

/// <summary>
/// Well-known values used when persisting self-serve company signup records.
/// </summary>
public static class SignupConstants
{
    /// <summary>
    /// Audit actor for records created through the prospect signup flow.
    /// </summary>
    public const string ProspectActor = "Prospect";

    /// <summary>
    /// Bigint audit actor id for <see cref="ProspectActor"/> on Glo tables.
    /// </summary>
    public const long ProspectActorUserId = 0;

    public const int DefaultLanguageId = 1;

    public const int TenantCompanyMasterEntityTypeId = 2;

    public const int DefaultInvitationExpiryDays = 7;

    public const int MinimumExpirationHours = 1;

    public const int TenantCodeSuffixAttempts = 5;

    public const string TenantAdminRoleCode = "TENANT_ADMIN";

    public const short TenantAdminGloRoleId = 7;

    /// <summary>
    /// Maps FGS varchar audit actors to bigint ids used by Glo tables.
    /// </summary>
    public static long? ToGloCreatedBy(string? createdBy)
    {
        if (string.IsNullOrWhiteSpace(createdBy))
        {
            return null;
        }

        if (long.TryParse(createdBy, out var userId))
        {
            return userId;
        }

        return string.Equals(createdBy, ProspectActor, StringComparison.Ordinal)
            ? ProspectActorUserId
            : null;
    }
}

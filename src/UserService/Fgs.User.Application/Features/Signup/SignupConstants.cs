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

    public const int DefaultLanguageId = 1;

    public const int TenantCompanyMasterEntityTypeId = 2;

    public const int DefaultInvitationExpiryDays = 7;

    public const int MinimumExpirationHours = 1;

    public const int TenantCodeSuffixAttempts = 5;
}
